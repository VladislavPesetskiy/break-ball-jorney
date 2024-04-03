using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Game.Modules.UIManager
{
    public sealed class UIScreenManager : MonoBehaviour, IUIScreenManager, IInitializable
    {
        [SerializeField]
        private Canvas contentLayer;
        
        [Inject]
        private readonly UIPanels panels;

        [Inject]
        private readonly DiContainer Container;

        private List<UIPanel<UIContext>> activePanels;
        
        private Dictionary<string, IPanelCommand> loadPanelCommands;

        public void Initialize()
        {
            activePanels = new List<UIPanel<UIContext>>();
            loadPanelCommands = new Dictionary<string, IPanelCommand>();
        }

        public bool IsPanelShowed<TPanel>() where TPanel : UIPanel<UIContext>
        {
            var panel = activePanels.SingleOrDefault(t => t.GetType() == typeof(TPanel));
            return panel != null && panel.IsActive;
        }

        public void ShowUIPanel<TPanel>(UIContext context = null, bool instant = false, Action<TPanel> panelShowed = null) where TPanel : UIPanel<UIContext>
        {
            if (activePanels.Exists(t => t.GetType() == typeof(TPanel)))
            {
                Debug.LogWarning($"Panel with type \'{typeof(TPanel)}\' already showed!");
                return;
            }

            var panelGuid = panels.TryGetPanelGuid<TPanel>();
            if (panelGuid != null)
            {
                var panelCommand = new LoadPanelCommand(panelGuid);
                loadPanelCommands.Add(panelGuid, panelCommand);

                var task = panelCommand.Execute<TPanel>();
                task.GetAwaiter().OnCompleted(() =>
                {
                    var result = task.GetAwaiter().GetResult();
                    ShowPanelAfterLoad(result, context, instant, panelShowed);
                });
            }
        }

        public async UniTask<TPanel> ShowUIPanelAsync<TPanel>(UIContext context = null, bool instant = false) where TPanel : UIPanel<UIContext>
        {
            if (activePanels.Exists(t => t.GetType() == typeof(TPanel)))
            {
                Debug.LogWarning($"Panel with type \'{typeof(TPanel)}\' already showed!");
                return null;
            }
            
            var panelGuid = panels.TryGetPanelGuid<TPanel>();
            if (panelGuid != null)
            {
                var panelCommand = new LoadPanelCommand(panelGuid);
                loadPanelCommands.Add(panelGuid, panelCommand);

                var result = await panelCommand.Execute<TPanel>();
                var panel = ShowPanelAfterLoad(result, context, instant);
                await UniTask.WaitWhile(() => panel.IsActive == false);
                return panel;
            }

            return null;
        }

        public void UpdateUIPanel<TPanel>(UIContext context)
        {
            var activePanel = activePanels.SingleOrDefault(t => t.GetType() == typeof(TPanel));
            
            if (activePanel == null)
            {
                Debug.LogWarning($"Panel with type \'{typeof(TPanel)}\' isn't showed!");
                return;
            }
            
            activePanel.UpdatePanel(context);
        }

        public void HideUIPanel<TPanel>(bool instant, Action panelHidden = null) where TPanel : UIPanel<UIContext>
        {
            var activePanel = activePanels.SingleOrDefault(t => t.GetType() == typeof(TPanel));
            
            if (activePanel != null)
            {
                activePanel.Hide(instant, () =>
                {
                    DisposePanel((TPanel)activePanel);
                    activePanels.Remove(activePanel);
                    panelHidden?.Invoke();
                });
                return;
            }

            DisposePanel((TPanel)activePanel);
            panelHidden?.Invoke();
        }

        public async UniTask HideUIPanelAsync<TPanel>(bool instant = false) where TPanel : UIPanel<UIContext>
        {
            var activePanel = activePanels.SingleOrDefault(t => t.GetType() == typeof(TPanel));
            if (activePanel != null)
            {
                activePanel.Hide(instant);
                
                await UniTask.WaitWhile(() => activePanel.IsActive);
                activePanels.Remove(activePanel);
            }

            DisposePanel((TPanel)activePanel);
        }

        public bool TryGetUIPanel<TPanel>(out TPanel panel) where TPanel : UIPanel<UIContext>
        {
            throw new NotImplementedException();
        }

        private TPanel ShowPanelAfterLoad<TPanel>
            (TPanel uiPanel, UIContext context, bool showInstant = false, Action<TPanel> panelShowed = null)
            where TPanel : UIPanel<UIContext>
        {
            var instantiate = Container.InstantiatePrefab(uiPanel, contentLayer.transform);
            var panel = instantiate.GetComponent<TPanel>();

            panel.Initialize(context);
            panel.Show(showInstant, () =>
            {
                panelShowed?.Invoke(panel);
            });
            activePanels.Add(panel);
            return panel;
        }

        private void DisposePanel<TPanel>(TPanel panelInstance = null) where TPanel : UIPanel<UIContext>
        {
            var panelCommand = GetPanelCommand<TPanel>();
            var key = loadPanelCommands.FirstOrDefault(t => t.Value == panelCommand).Key;
            panelCommand.Dispose();
            loadPanelCommands.Remove(key);

            if (panelInstance != null)
            {
                panelInstance.DeInitialize();
                Destroy(panelInstance.gameObject);
            }
        }
        
        private IPanelCommand GetPanelCommand<TPanel>() where TPanel : UIPanel<UIContext>
        {
            var panelGuid = panels.TryGetPanelGuid<TPanel>();
            if (panelGuid != null && loadPanelCommands.ContainsKey(panelGuid))
            {
                return loadPanelCommands[panelGuid];
            }
            
            Debug.LogWarning($"Panel with type \'{typeof(TPanel)}\' already hidden!");
            return null;
        }
    }
}

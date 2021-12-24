using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/**********************************************
* 模块名: DeserializeTool.cs
* 功能描述：实验跳步，步骤场景还原实现
***********************************************/

public class DeserializeTool : MonoBehaviour
{
    [Header("放入所有重置信息")]
    public RestoreAsset[] resetInfos;

    #region Private Fields

    // 记录不需要进行重置的字段名
    private List<string> bannedName = new List<string>();

    // 记录不需要进行重置的类型
    private static HashSet<Type> unrestoredTypes = new HashSet<Type>();

    // 记录需要自定义重置方法的 类型-恢复方法 字典
    private static Dictionary<Type, Delegate> typeRestoreDict = new Dictionary<Type, Delegate>();

    // 记录不需要进行重置物体名
    private static HashSet<string> unrestoredNames = new HashSet<string>();

    // 记录不需要进行重置的父物体名（所有子孙物体都不会进行重置）
    private static HashSet<string> unrestoredParentNames = new HashSet<string>();

    // 用于保存prefab中物体组件的容器
    private static List<Component> sourceComponentList = new List<Component>();

    // 用于运行时场景prefab中物体组件的容器
    private static List<Component> targetComponentList = new List<Component>();

    // 用于BasicRestore的类型
    private static HashSet<Type> basicRestoreTypes = new HashSet<Type>();

    private GameObject uiCanvas, ObjectCamera;

    #endregion Private Fields

    /// <summary>
    /// 初始化不进行还原的属性字段
    /// </summary>
    public void InitList()
    {
        unrestoredTypes.Clear();
        unrestoredNames.Clear();
        typeRestoreDict.Clear();
        unrestoredParentNames.Clear();
        AddUnrestoredNames();
        AddUnresotredTypes();
        AddUnresotredParentNames();
        AddBannedList();
        AddCustomRestoreDelegate();
        AddBasicResotreComponents();
    }

    #region 添加不进行还原的组件和属性

    /// <summary>
    /// 添加不进行还原的属性(Property)
    /// </summary>
    private void AddBannedList()
    {
        bannedName.Clear();
        /*
         * 填写不进恢复的字段名
         * */
        bannedName.Add("randomSeed");
        bannedName.Add("useAutoRandomSeed");
    }

    /// <summary>
    /// 请在该方法内添加不需要重置或者重置会出现异常的类型
    /// </summary>
    private void AddUnresotredTypes()
    {
        /*
         * 填写不进恢复的类型
         */

        unrestoredTypes.Add(typeof(HighlightingSystem.HighlighterRenderer));
        unrestoredTypes.Add(typeof(HighlightingSystem.HighlightingBase));
        unrestoredTypes.Add(typeof(HighlightingSystem.HighlightingRenderer));
        unrestoredTypes.Add(typeof(UnityEngine.Rendering.PostProcessing.PostProcessLayer));
        unrestoredTypes.Add(typeof(UnityEngine.Rendering.PostProcessing.PostProcessVolume));
        unrestoredTypes.Add(typeof(Animator));
        unrestoredTypes.Add(typeof(AddInteraction));
        unrestoredTypes.Add(typeof(RemoveInteraction));
        unrestoredTypes.Add(typeof(MoveCardInteraction));
        unrestoredTypes.Add(typeof(CameraControl));
        unrestoredTypes.Add(typeof(UnityEngine.Video.VideoPlayer));
        unrestoredTypes.Add(typeof(ReflectionProbe));

        // 如果场景内使用了BezierMesh生成器，请将下列代码取消注释.
        //unrestoredTypes.Add(typeof(AssemblySystem.Connection));
        //unrestoredTypes.Add(typeof(AssemblySystem.Device));
        //unrestoredTypes.Add(typeof(AssemblySystem.Port));
        //unrestoredTypes.Add(typeof(ConnectionRenderer));
        //unrestoredTypes.Add(typeof(BezierPipeRenderer));
    }

    /// <summary>
    /// 请在该方法添加跳过重置的物体(其下的子物体会进行重置）
    /// </summary>
    private void AddUnrestoredNames()
    {
        /*
         * 填写不进恢复的物体名，会跳过物体，其子物体还是会进行重置
         */
    }

    /// <summary>
    /// 请在该方法添加跳过重置的物体（因为采用的是递归重置，若跳过父物体，其下的子物体也不会进行重置）
    /// </summary>
    private void AddUnresotredParentNames()
    {
        /*
         * 填写不进恢复的父物体名，其及其下子孙物体都不会进行重置
         */
        unrestoredParentNames.Add("NotOftenModify");
        unrestoredParentNames.Add("Anchor_TopRight");
        unrestoredParentNames.Add("Anchor_TopLeft");
        unrestoredParentNames.Add("UIScene_Begin");
        unrestoredParentNames.Add("ControlCenter");
        unrestoredParentNames.Add("ChemistryLab");
        unrestoredParentNames.Add("shengwushiyanshi");
        unrestoredParentNames.Add("PhysicsLab");
        unrestoredParentNames.Add("UIScene_Step");
        unrestoredParentNames.Add("StageCard");
        unrestoredParentNames.Add("Anchor_BottomLeft");
        unrestoredParentNames.Add("Anchor_TopRight");
        unrestoredParentNames.Add("Anchor_TopLeft");
    }

    /// <summary>
    /// 添加自定义的类和恢复方法，需要自己实现RestoreComponent方法。目前是在ProcessData的ExternalMehods类中写了扩展方法。
    /// </summary>
    private void AddCustomRestoreDelegate()
    {
        /*
         * 字典Add(Type, Action<ComponentType, ComponentType>)
         * 这里使用调用扩展方法的匿名函数
         * 扩展方法在ProcessData.cs中
         */

        typeRestoreDict.Add(typeof(HighlightingSystem.Highlighter),
            new Action<HighlightingSystem.Highlighter, HighlightingSystem.Highlighter>((target, source) =>
            {
                target.RestoreComponent(source);
            }));

        typeRestoreDict.Add(typeof(Camera),
            new Action<Camera, Camera>((target, source) =>
            {
                target.RestoreComponent(source);
            }));

        typeRestoreDict.Add(typeof(SkinnedMeshRenderer),
            new Action<SkinnedMeshRenderer, SkinnedMeshRenderer>((target, source) =>
            {
                target.RestoreComponent(source);
            }));

        typeRestoreDict.Add(typeof(ParticleSystem),
            new Action<ParticleSystem, ParticleSystem>((target, source) =>
            {
                target.RestoreComponent(source);
            }));

        typeRestoreDict.Add(typeof(Transform),
            new Action<Transform, Transform>((target, source) =>
            {
                target.RestoreComponent(source);
            }));

        typeRestoreDict.Add(typeof(UnityEngine.UI.Image),
            new Action<UnityEngine.UI.Image, UnityEngine.UI.Image>((target, source) =>
            {
                target.RestoreComponent(source);
            }));

        typeRestoreDict.Add(typeof(UnityEngine.UI.Text),
            new Action<UnityEngine.UI.Text, UnityEngine.UI.Text>((target, source) =>
            {
                target.RestoreComponent(source);
            }));

        typeRestoreDict.Add(typeof(SpriteRenderer),
            new Action<SpriteRenderer, SpriteRenderer>((target, source) =>
            {
                target.RestoreComponent(source);
            }));

        typeRestoreDict.Add(typeof(RectTransform),
            new Action<RectTransform, RectTransform>((target, source) =>
            {
                target.RestoreComponent(source);
            }));

        typeRestoreDict.Add(typeof(HandCardControl),
            new Action<HandCardControl, HandCardControl>((target, source) =>
            {
                target.RestoreComponent(source);
            }));
    }

    private void AddBasicResotreComponents()
    {
        basicRestoreTypes.Add(typeof(UnityEngine.UI.Image));
        basicRestoreTypes.Add(typeof(UnityEngine.UI.Text));
        basicRestoreTypes.Add(typeof(UnityEngine.UI.RawImage));
        basicRestoreTypes.Add(typeof(RectTransform));
        basicRestoreTypes.Add(typeof(HandCardControl));
    }

    #endregion 添加不进行还原的组件和属性

    #region MonoBehaviour

    private void Start()
    {
        uiCanvas = GameObject.Find("UICanvas");
        ObjectCamera = GameObject.Find("3DObjectsCamera");
        InitList();
    }

    #endregion MonoBehaviour

    #region Helper Functions

    #region Recursive Function 深度优先递归方法

    /// <summary>
    /// 递归进行重置
    /// </summary>
    /// <param name="target"></param>
    /// <param name="source"></param>
    public void ResetRecursively(Transform target, Transform source)
    {
        if (target == null || source == null)
        {
            Debug.LogError("ResetRecursivelyBasic Error, Transform" + target + " is null!");
            return;
        }

        // 跳出递归，该物体和其子物体都不会进行重置
        if (unrestoredParentNames.Contains(target.gameObject.name))
            return;

        if (!unrestoredNames.Contains(target.gameObject.name))
            ResetAction(target, source);

        if (target.childCount == source.childCount)
        {
            for (int i = 0; i < target.childCount; i++)
            {
                var target_child = target.GetChild(i);
                var source_child = source.GetChild(i);
                ResetRecursively(target_child, source_child);
            }
        }
        else
            Debug.LogError(target.name + "中子物体数量不匹配！");
    }

    /// <summary>
    /// 递归进行基础重置
    /// </summary>
    /// <param name="target"></param>
    /// <param name="source"></param>
    public void ResetRecursivelyBasic(Transform target, Transform source)
    {
        if (target == null || source == null)
        {
            Debug.LogError("ResetRecursivelyBasic Error, Transform" + target + " is null!");
            return;
        }

        // 跳出递归，该物体和其子物体都不会进行重置
        if (unrestoredParentNames.Contains(target.gameObject.name))
            return;

        // 如果unrestoredNames包含此物体名，则不进行重置该物体，但是其子物体都会进行重置
        if (!unrestoredNames.Contains(target.gameObject.name))
            ResetActionBasic(target, source);

        if (target.childCount == source.childCount)
        {
            for (int i = 0; i < target.childCount; i++)
            {
                var target_child = target.GetChild(i);
                var source_child = source.GetChild(i);
                ResetRecursivelyBasic(target_child, source_child);
            }
        }
        else
            Debug.LogError(target.name + "中子物体数量不匹配！");
    }

    #endregion Recursive Function 深度优先递归方法

    #region Recursive Action 递归对每个GameObject节点进行的动作

    /// <summary>
    /// 重置方法的整合（重置激活状态、重置位置旋转、添加脚本到重置列表）
    /// </summary>
    /// <param name="target"></param>
    /// <param name="source"></param>
    public void ResetAction(Transform target, Transform source)
    {
        ResetActivation(target, source);
        AddComponentsToList(target.gameObject, targetComponentList);
        AddComponentsToList(source.gameObject, sourceComponentList);
        if (targetComponentList.Count != sourceComponentList.Count)
        {
            Debug.LogError("Target and source components number not match in " + target.name + ", " + transform.parent);
            return;
        }
    }

    /// <summary>
    /// 基础重置方法的整合（重置激活状态、重置位置旋转）
    /// </summary>
    /// <param name="target"></param>
    /// <param name="source"></param>
    private void ResetActionBasic(Transform target, Transform source)
    {
        ResetActivation(target, source);
        AddComponentsToListBasic(target.gameObject, targetComponentList);
        AddComponentsToListBasic(source.gameObject, sourceComponentList);
        if (targetComponentList.Count != sourceComponentList.Count)
        {
            Debug.LogError("Target and source components number not match in " + target.name + ", " + transform.parent);
            return;
        }
    }

    #endregion Recursive Action 递归对每个GameObject节点进行的动作

    #region Restore Functions

    /// <summary>
    /// 根据配置重新激活卡牌
    /// </summary>
    /// <param name="resetInfo"></param>
    private void RestoreAvailableCard(RestoreAsset resetInfo)
    {
        DisableAllCards();
        foreach (var cardIndex in resetInfo.activeCard)
            OneFlyLib.ManagerEvent.Call(OneFlyLib.Tips.SetCardsAvailable, true, cardIndex);
    }

    /// <summary>
    /// 根据resetInfo中的信息重置ControlCenter中的卡牌模型绑定情况
    /// </summary>
    /// <param name="resetInfo"></param>
    private void ResetControlCenter(RestoreAsset resetInfo)
    {
        for (int i = 0; i < resetInfo.objs.Count; ++i)
        {
            var ui = (resetInfo.objs[i].UIName == "" ?
                null : uiCanvas.transform.FindChildRecursively(resetInfo.objs[i].UIName));
            var model = (resetInfo.objs[i].objName == "" ?
                null : ObjectCamera.transform.FindChildRecursively(resetInfo.objs[i].objName));

            if (ui == null)
            {
                Debug.LogError("恢复ControlCenter绑定属性时未找到对应下屏物体，请检查");
            }
            CourseControl.instance.objs[i].ui = ui != null ? ui.gameObject : null;
            CourseControl.instance.objs[i].model = model != null ? model.gameObject : null;
        }
    }

    /// <summary>
    /// 根据resetInfo中的信息重置步骤进度
    /// </summary>
    /// <param name="resetInfo"></param>
    private void RestoreStateProgress(RestoreAsset resetInfo)
    {
        StateController.Instance.SetCourseProgress(resetInfo.progress);
    }

    /// <summary>
    /// 重置激活状态
    /// </summary>
    /// <param name="target"></param>
    /// <param name="origin"></param>
    private void ResetActivation(Transform target, Transform origin)
    {
        var is_activated = origin.gameObject.activeSelf;
        target.gameObject.SetActive(is_activated);
    }

    /// <summary>
    /// 重置物体位置和旋转
    /// </summary>
    /// <param name="target"></param>
    /// <param name="origin"></param>
    private void ResetTransform(Transform target, Transform origin)
    {
        target.position = origin.position;
        target.rotation = origin.rotation;
        target.localScale = origin.localScale;
    }

    #endregion Restore Functions

    #region RestoreHelpers

    /// <summary>
    /// 复制组件的属性和字段的值
    /// </summary>
    /// <param name="targetList"></param>
    /// <param name="sourceList"></param>
    /// <param name="bannedName"></param>
    public static void ComponentListCopyValue(List<Component> targetList, List<Component> sourceList, List<string> bannedName = null)
    {
        if (targetList.Count != sourceList.Count)
            Debug.LogError("Restored Component list's size dont match!");

        for (int i = 0; i < targetList.Count; ++i)
        {
            var prefabComponent = sourceList[i];
            var runtimeComponent = targetList[i];

            Type componentType = prefabComponent.GetType();

            try
            {
                // 如果组件类型不一致则直接返回
                if (targetList[i].GetType() != sourceList[i].GetType())
                    continue;

                // 调用自定义的恢复方法
                if (typeRestoreDict.ContainsKey(componentType))
                {
                    typeRestoreDict[componentType].DynamicInvoke(runtimeComponent, prefabComponent);
                    continue;
                }

                // 如果该类型没有定义自定义恢复方法且在不恢复类型中，则跳过
                else if (unrestoredTypes.Contains(componentType))
                    continue;

                // 默认恢复方法，进行反射进行逐个字段属性恢复

                // Restore Fields.
                var prefabVars = prefabComponent.GetType().GetRuntimeProperties();
                var runtimeVars = runtimeComponent.GetType().GetRuntimeProperties();

                // Get Enumerator from the ienumerables of Runtimefields
                var prefabVarIt = prefabVars.GetEnumerator();
                var runtimeVarIt = runtimeVars.GetEnumerator();

                while (prefabVarIt.MoveNext() && runtimeVarIt.MoveNext())
                {
                    var prefabProperty = prefabVarIt.Current;
                    var runtimeProperty = runtimeVarIt.Current;
                    if (prefabProperty.IsDefined(typeof(ObsoleteAttribute), true) || !prefabProperty.CanWrite || !prefabProperty.CanRead)
                        continue;

                    if (prefabProperty.Name == runtimeProperty.Name)
                    {
                        if (bannedName == null || !bannedName.Contains(runtimeProperty.Name))
                            runtimeProperty.SetValue(runtimeComponent, prefabProperty.GetValue(prefabComponent));
                    }
                }

                // Restore Property.
                var prefabPros = prefabComponent.GetType().GetRuntimeFields();
                var runtimePros = runtimeComponent.GetType().GetRuntimeFields();

                // Get Enumerator from the ienumerables of Runtimefields

                var prefabProIt = prefabPros.GetEnumerator();
                var runtimeProIt = runtimePros.GetEnumerator();
                while (prefabProIt.MoveNext() && runtimeProIt.MoveNext())
                {
                    var prefabField = prefabProIt.Current;
                    var runtimeField = runtimeProIt.Current;
                    if (!(!prefabField.Attributes.HasFlag(FieldAttributes.Literal) &&
                                    !prefabField.Attributes.HasFlag(FieldAttributes.Static) &&
                                    !prefabField.Attributes.HasFlag(FieldAttributes.Private)))
                        continue;

                    if (prefabField.Name != runtimeField.Name)
                        continue;

                    runtimeField.SetValue(runtimeComponent, prefabField.GetValue(prefabComponent));
                }
            }
            catch (Exception e)
            {
                Debug.LogError("恢复物体" + prefabComponent.gameObject.name + "的" + prefabComponent.name + "组件时出现异常:" + e.Message);
                continue;
            }
        }
    }

    /// <summary>
    /// 添加Component到指定List当中
    /// </summary>
    /// <param name="target"></param>
    /// <param name="targetList"></param>
    private void AddComponentsToList(GameObject target, List<Component> targetList)
    {
        var components = target.GetComponents(typeof(Component));
        foreach (var component in components)
            if (!unrestoredTypes.Contains(component.GetType()))
                targetList.Add(component);
    }

    /// <summary>
    /// 添加Component到指定List当中
    /// </summary>
    /// <param name="target"></param>
    /// <param name="targetList"></param>
    private void AddComponentsToListBasic(GameObject target, List<Component> targetList)
    {
        var components = target.GetComponents(typeof(Component));
        foreach (var component in components)
            if (basicRestoreTypes.Contains(component.GetType()))
                targetList.Add(component);
    }

    /// <summary>
    /// 实例创建重置模板
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    private Transform InstantiatePrefab(GameObject prefab)
    {
        var new_prefab = Instantiate(prefab);
        new_prefab.name = prefab.name;
        return new_prefab.transform;
    }

    #endregion RestoreHelpers

    #region 卡牌激活及模型绑定重置方法

    /// <summary>
    /// 禁用所有卡牌
    /// </summary>
    private void DisableAllCards()
    {
        for (int i = 0; i < CourseControl.instance.objs.Count; ++i)
            OneFlyLib.ManagerEvent.Call(OneFlyLib.Tips.SetCardsAvailable, false, i);
    }

    #endregion 卡牌激活及模型绑定重置方法

    #endregion Helper Functions

    #region Public Interfaces

    /// <summary>
    /// 重置接口， 接受重置的步骤数，0-indexed
    /// </summary>
    /// <param name="index"></param>
    public void RestoreState(int stateIndex)
    {
        // index为数组下标
        int index = -1;

        // 搜索数组下标
        for (int i = 0; i < resetInfos.Length; i++)
            if (resetInfos[i].stateIndex == stateIndex)
                index = i;

        sourceComponentList.Clear();
        targetComponentList.Clear();

        if (index < 0)
        {
            Debug.LogError("未找到步骤编号下标为" + stateIndex.ToString() + "的ResetInfo");
            return;
        }

        if (resetInfos[index].UICanvasPrefab)
        {
            // 注释掉的为重置BasicFrameWork的部分
            var basicFrameworkPrefab = InstantiatePrefab(resetInfos[index].UICanvasPrefab);
            basicFrameworkPrefab.transform.name = "UICanvas";
            ResetRecursivelyBasic(uiCanvas.transform, basicFrameworkPrefab);
            Destroy(basicFrameworkPrefab.gameObject);
        }

        if (resetInfos[index].ObjectCameraPrefab)
        {
            var objectCameraPrefab = InstantiatePrefab(resetInfos[index].ObjectCameraPrefab);
            objectCameraPrefab.transform.name = "3DObjectsCamera";
            ResetRecursively(ObjectCamera.transform, objectCameraPrefab);
            Destroy(objectCameraPrefab.gameObject);
        }

        ComponentListCopyValue(targetComponentList, sourceComponentList, bannedName);
        ResetControlCenter(resetInfos[index]);
        RestoreAvailableCard(resetInfos[index]);
        RestoreStateProgress(resetInfos[index]);
    }

    #endregion Public Interfaces
}

/**********************************************
* 模块名: ExtendedRestoreMethods.cs
* 功能描述：为不同组件添加自定义恢复扩展方法的扩展类
***********************************************/

public static class ExtendedRestoreMethods
{
    /// <summary>
    /// SkinnedMeshRenderer扩展方法，对SkinnedMeshRenderer进行变形器逐个Blend值恢复
    /// </summary>
    /// <param name="target"></param>
    /// <param name="origin"></param>
    public static void RestoreComponent(this SkinnedMeshRenderer target,
                                        SkinnedMeshRenderer origin)
    {
        var shapeCount = origin.sharedMesh.blendShapeCount;

        for (int i = 0; i < shapeCount; ++i)
        {
            target.SetBlendShapeWeight(i, origin.GetBlendShapeWeight(i));
        }
    }

    /// <summary>
    /// Transform扩展方法，对Transform进行Position, Rotation, Scale进行恢复
    /// </summary>
    /// <param name="target"></param>
    /// <param name="origin"></param>
    public static void RestoreComponent(this Transform target,
                                        Transform orgin)
    {
        target.localPosition = orgin.localPosition;
        target.rotation = orgin.rotation;
        target.localScale = orgin.localScale;
    }

    /// <summary>
    /// ParticleSystem扩展方法(待实现)
    /// </summary>
    /// <param name="target"></param>
    /// <param name="orgin"></param>
    public static void RestoreComponent(this ParticleSystem target,
                                        ParticleSystem orgin)
    {
        // TODO Implementation.
    }

    /// <summary>
    /// Camera扩展方法，对Camera进行Field of View属性恢复
    /// </summary>
    /// <param name="target"></param>
    /// <param name="origin"></param>
    public static void RestoreComponent(this Camera target,
                                        Camera origin)
    {
        target.fieldOfView = origin.fieldOfView;
    }

    /// <summary>
    /// Highlighter扩展方法，对tween属性进行恢复
    /// </summary>
    /// <param name="target"></param>
    /// <param name="origin"></param>
    public static void RestoreComponent(this HighlightingSystem.Highlighter target,
                                        HighlightingSystem.Highlighter origin)
    {
        target.tween = origin.tween;
    }

    public static void RestoreComponent(this UnityEngine.UI.Image target,
                                        UnityEngine.UI.Image origin)
    {
        target.sprite = origin.sprite;
        target.color = origin.color;
    }

    public static void RestoreComponent(this UnityEngine.UI.Text target,
                                        UnityEngine.UI.Text origin)
    {
        target.text = origin.text;
        target.color = origin.color;
    }

    public static void RestoreComponent(this SpriteRenderer target,
                                        SpriteRenderer origin)
    {
        target.sprite = origin.sprite;
        target.color = origin.color;
    }

    public static void RestoreComponent(this RectTransform target,
                                        RectTransform origin)
    {
        target.anchoredPosition = origin.anchoredPosition;
        target.rotation = origin.rotation;
        target.localScale = origin.localScale;
    }
    public static void RestoreComponent(this HandCardControl target,
                                        HandCardControl origin)
    {
        target.startCount = origin.startCount;
        target.triggeredOnce = origin.triggeredOnce;
        target.timer = origin.timer;
        target.startOperation = origin.startOperation;
    }
}
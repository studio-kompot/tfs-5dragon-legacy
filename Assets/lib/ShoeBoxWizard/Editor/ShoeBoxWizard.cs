using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.Collections.Generic;
using System;
using System.Collections;

namespace Samana.ShoeBox
{
    //TODO если разрезаю спрайтЛист средствами Юнити, а потом подгружаю этот лист и хмл и пытаюсь нарезать, то возникают ошибки.
    public class ShoeBoxWizard : EditorWindow
    {
        Sprite _spriteSheet;
        TextAsset _xml;

        Dictionary<string, ClipData> _clipsData;
        string _pathCopyBuffer = "";

        // settings
        bool _showSettings;
        bool _disableAllClips;
        bool _disableWithOneFrame;

        bool _enableFPSChange;
        FPS _allClipsFPS = FPS._30;
        int _customFPS = 30;

        bool _enableSavePath;
        string _pathSaveClips = "Assets";

        bool _enableChangeLoop;
        bool _loopClips = true;

        bool _legacy;

        bool _enableAlignment;
        SpriteAlignment _alignment = SpriteAlignment.Center;
        Vector2 _pivot = Vector2.zero;
        bool _showPivots;
        // end settings

        Vector2 _scrollPosition = Vector2.zero;
        float _maxPreviewClipSide = 150f;// max label (texture) size in preview clip
        GUIStyle _middleAligment;

        //preview clips sort data
        bool _sortAnimExist;
        bool _sortByName;

        // helper gui enabled
        bool _oldGUIEnable;
        bool _countGUIEnabled;

        // for pivot blinking
        Color _pivotColor;
        float _colorLerpTime = 0;
        float _pivotColorFPSUpdate = 1f / 10f;//10 fps
        float _nextFrame;

        //some pivot is changed
        bool _somePivotIsChanged = false;

        static ShoeBoxWizard window;

        [MenuItem("Window/ShoeBoxWizard")]
        static void showWindow()
        {
            window = GetWindow<ShoeBoxWizard>("ShoeBoxWizard");
            window.minSize = new Vector2(720, 200);
            window.Show();
        }

        //[MenuItem("Window/ShoeBoxWizard_close")]
        static void closeWindow()
        {
            if (window) window.Close();
        }

        void OnEnable()
        {
            //Debug.Log("enable");
            _clipsData = new Dictionary<string, ClipData>();
            _middleAligment = new GUIStyle();
            _middleAligment.alignment = TextAnchor.MiddleCenter;

            _nextFrame = Time.realtimeSinceStartup + _pivotColorFPSUpdate;

            _somePivotIsChanged = false;
            ClipData.PIVOT_CHANGED = PIVOT_CHANGE_Handler;
        }

        void OnDisable()
        {
            //Debug.Log("disable");
            ClipData.PIVOT_CHANGED = null;
            resetAll();
            window = null;
        }

        void OnGUI()
        {
            //showDebugInfo();
            //return;
            showTopMainFieldsAndButtons();
            showSaveButtons();

            if (_clipsData != null && _clipsData.Values.Count != 0)
            {
                drawClipsAnimations();
            }

            drawHelpButton();
        }

        void Update()
        {
            if (_clipsData == null || _clipsData.Values.Count == 0) return;

            foreach (ClipData clipData in _clipsData.Values)
            {
                clipData.updateFrame();
            }

            // blink for pivot color
            if (Time.realtimeSinceStartup > _nextFrame)
            {
                _pivotColor = Color.Lerp(Color.white, Color.black, _colorLerpTime);
                _colorLerpTime += 0.3f;
                if (_colorLerpTime > 1f) _colorLerpTime = 0;
                _nextFrame = Time.realtimeSinceStartup + _pivotColorFPSUpdate;
            }


            Repaint();
        }


        private void showTopMainFieldsAndButtons()
        {
            EditorGUILayout.BeginHorizontal("box");
            {
                EditorGUILayout.BeginVertical();
                {
                    drawSpriteSheetAndXMLFields();
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(_middleAligment);
                {
                    EditorGUILayout.Space();
                    draw_CUT_PREVIEW_RESET_buttons();
                    EditorGUILayout.Space();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void showSaveButtons()
        {
            EditorGUILayout.BeginHorizontal();
            {
                Color oldColor = GUI.backgroundColor;
                GUI.backgroundColor = Color.yellow;
                setGUIEnable(_clipsData.Values.Count != 0);
                if (GUILayout.Button("SAVE ALL CLIPS")) saveClips();

                if (GUILayout.Button("SAVE CLIPS WITH ANIMATION ONLY")) saveClips(true);

                if (GUILayout.Button("JUST UPDATE CUSTOM PIVOTS IN SPRITESHEET")) rebuildSpritePivotsInSpriteSheet();
                restoreGUIEnabled();

                GUI.backgroundColor = oldColor;
            }
            EditorGUILayout.EndHorizontal();
            restoreGUIEnabled();
        }

        private void drawSpriteSheetAndXMLFields()
        {
            EditorGUILayout.PrefixLabel("Sprite Sheet IMAGE:");
            GUI.changed = false;
            _spriteSheet = (Sprite)EditorGUILayout.ObjectField(_spriteSheet, typeof(Sprite), false);
            if (GUI.changed)
            {
                _clipsData.Clear();
            }

            EditorGUILayout.PrefixLabel("Sprite Sheet XML:");
            GUI.changed = false;
            _xml = (TextAsset)EditorGUILayout.ObjectField(_xml, typeof(TextAsset), false);
            if (GUI.changed)
            {
                _clipsData.Clear();
            }

            bool isXmlFormat = Path.GetExtension(AssetDatabase.GetAssetPath(_xml)) == ".xml";
            if (_xml != null && !isXmlFormat)
            {
                EditorGUILayout.HelpBox("Sprite Sheet XML field contains not .xml file!", MessageType.Warning);
            }

        }

        private void draw_CUT_PREVIEW_RESET_buttons()
        {
            bool isXmlFormat = Path.GetExtension(AssetDatabase.GetAssetPath(_xml)) == ".xml";

            setGUIEnable(_spriteSheet != null && _xml != null && isXmlFormat);
            if (GUILayout.Button("CUT SPRITESHEET and PREVIEW CLIPS"))
            {
                //Debug.Log("cut");
                cutSpriteSheet();
            }
            restoreGUIEnabled();

            setGUIEnable(_spriteSheet != null);
            if (GUILayout.Button("CREATE PRIVIEW CLIPS"))
            {
                if (_spriteSheet != null) createClipData(true);
            }
            restoreGUIEnabled();

            setGUIEnable(_spriteSheet != null || _xml != null);
            if (GUILayout.Button("BEGIN NEW (reset all)"))
            {
                resetAll();
            }
            restoreGUIEnabled();
        }

        private void drawSettingsButton()
        {
            // SETTINGS
            if (_clipsData != null && _clipsData.Count != 0)
            {
                Color oldColor = GUI.backgroundColor;
                GUI.backgroundColor = new Color(0.8f, 0.89f, 0.8f);
                if (GUILayout.Button("settings show/hide", GUILayout.ExpandWidth(false)))
                {
                    _showSettings = !_showSettings;
                }
                GUI.backgroundColor = oldColor;
            }
        }

        private void drawSettings()
        {
            bool oldEnable;

            Color oldColor = GUI.backgroundColor;
            GUI.backgroundColor = new Color(0.8f, 0.89f, 0.8f);

            EditorGUILayout.BeginHorizontal("box");
            {
                GUI.backgroundColor = oldColor;

                //                                  COLUMN 1
                EditorGUILayout.BeginVertical();
                {
                    _disableAllClips = EditorGUILayout.ToggleLeft("disable all clips", _disableAllClips);
                    _disableWithOneFrame = EditorGUILayout.ToggleLeft("disable clips with ONE frame only", _disableWithOneFrame);
                    _legacy = EditorGUILayout.ToggleLeft("legacy all clips", _legacy);
                    EditorGUILayout.Space();

                    // alignment
                    _showPivots = EditorGUILayout.ToggleLeft("show all pivots", _showPivots);

                    oldEnable = GUI.enabled;
                    _enableAlignment = EditorGUILayout.ToggleLeft("enable alignment", _enableAlignment);
                    GUI.enabled = _enableAlignment;
                    EditorGUILayout.BeginHorizontal();
                    {
                        _alignment = (SpriteAlignment)EditorGUILayout.EnumPopup(_alignment, GUILayout.Width(100));
                        if (_alignment == SpriteAlignment.Custom)
                        {
                            bool oldWide = EditorGUIUtility.wideMode;
                            EditorGUIUtility.wideMode = true;
                            _pivot = EditorGUILayout.Vector2Field("", _pivot, GUILayout.Width(100));
                            EditorGUIUtility.wideMode = oldWide;
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    GUI.enabled = oldEnable;

                }
                EditorGUILayout.EndVertical();

                //                                  COLUMN 2
                EditorGUILayout.BeginVertical();
                {
                    // loop
                    oldEnable = GUI.enabled;
                    _enableChangeLoop = EditorGUILayout.ToggleLeft("enable loop switch", _enableChangeLoop, GUILayout.ExpandWidth(false));
                    GUI.enabled = _enableChangeLoop;
                    EditorGUILayout.BeginHorizontal();
                    {
                        makeSpaceTab(20);
                        _loopClips = EditorGUILayout.ToggleLeft("loop all clips", _loopClips);
                    }
                    EditorGUILayout.EndHorizontal();
                    GUI.enabled = oldEnable;
                    EditorGUILayout.Space();

                    // FPS
                    _enableFPSChange = EditorGUILayout.ToggleLeft("enable change FPS for all clips", _enableFPSChange);

                    oldEnable = GUI.enabled;
                    GUI.enabled = _enableFPSChange;

                    EditorGUILayout.BeginHorizontal();
                    {
                        makeSpaceTab(20);
                        _allClipsFPS = (FPS)EditorGUILayout.EnumPopup(_allClipsFPS, GUILayout.Width(100));

                        if (_allClipsFPS == FPS.custom)
                            _customFPS = EditorGUILayout.IntField(_customFPS, GUILayout.Width(50));
                        else
                            _customFPS = (int)_allClipsFPS;
                    }
                    EditorGUILayout.EndHorizontal();
                    GUI.enabled = oldEnable;
                    EditorGUILayout.Space();

                    // path to save clips
                    _enableSavePath = EditorGUILayout.ToggleLeft("enable save path for all clips", _enableSavePath);
                    oldEnable = GUI.enabled;
                    GUI.enabled = _enableSavePath;
                    EditorGUILayout.BeginHorizontal();
                    {
                        makeSpaceTab(20);
                        if (GUILayout.Button("change path", GUILayout.ExpandWidth(false)))
                        {
                            _pathSaveClips = EditorUtility.OpenFolderPanel("Save clip in folder", "Assets", "");
                            if (string.IsNullOrEmpty(_pathSaveClips)) _pathSaveClips = "Assets";
                            else _pathSaveClips = "Assets" + _pathSaveClips.Substring(Application.dataPath.Length);
                        }

                        EditorGUILayout.LabelField(_pathSaveClips);
                    }
                    EditorGUILayout.EndHorizontal();
                    GUI.enabled = oldEnable;
                    EditorGUILayout.Space();
                }
                EditorGUILayout.EndVertical();

                //                                  COLUMN 3
                EditorGUILayout.BeginVertical();
                {
                    //                              APPLY
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    if (GUILayout.Button("APPLY SETTINGS", GUILayout.Width(110), GUILayout.Height(100)))
                    {
                        foreach (ClipData clipData in _clipsData.Values)
                        {
                            // on/off
                            clipData.isCreateClip = !_disableAllClips;
                            // on/off with one frame
                            if (clipData.sprites.Count == 1) clipData.isCreateClip = !_disableWithOneFrame && !_disableAllClips;
                            // legacy
                            clipData.legacy = _legacy;
                            // loop
                            if (_enableChangeLoop) clipData.loop = _loopClips;
                            //alignment
                            if (_enableAlignment)
                            {
                                if (_alignment != SpriteAlignment.Custom) clipData.spriteAlignment = _alignment;
                                else
                                {
                                    clipData.spriteAlignment = SpriteAlignment.Custom;
                                    clipData.pivot = _pivot;
                                }
                            }
                            clipData.showPivot = _showPivots;
                            // fps
                            if (_enableFPSChange)
                            {
                                if (_allClipsFPS != FPS.custom)
                                    clipData.fpsEnum = _allClipsFPS;
                                else
                                {
                                    clipData.fpsEnum = FPS.custom;
                                    clipData.fps = _customFPS;
                                }
                                clipData.updateFPS();
                            }
                            //path to save
                            if (_enableSavePath) clipData.savePath = _pathSaveClips;
                        }
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void drawClipsAnimations()
        {
            drawPreviewLabelAndHisSettings();

            // draw clips preview

            bool oldEnable;
            Color oldColor;

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            EditorGUILayout.BeginVertical();

            foreach (ClipData clipData in _clipsData.Values)
            {
                if (!clipData.isCreateClip) GUI.enabled = false;

                oldColor = GUI.backgroundColor;
                if (clipData.sprites.Count < 2)
                {
                    GUI.backgroundColor = new Color(1.00f, 0.94f, 0.98f, 1.00f);
                }

                EditorGUILayout.BeginHorizontal("box");
                {
                    // FRAME ANIMATION
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField(new GUIContent(clipData.textures[clipData.currentFrame]), _middleAligment, GUILayout.Width(_maxPreviewClipSide), GUILayout.Height(_maxPreviewClipSide));

                    // draw pivot
                    if (Event.current.type == EventType.Repaint) clipData.spriteLastRect = GUILayoutUtility.GetLastRect();

                    //drawPivotsHandles(clipData);


                    EditorGUILayout.Space();

                    EditorGUILayout.BeginVertical();
                    {
                        // CREATE CLIP
                        GUI.enabled = true;
                        clipData.isCreateClip = EditorGUILayout.Toggle("create clip:", clipData.isCreateClip);
                        if (!clipData.isCreateClip) GUI.enabled = false;


                        // CLIP NAME
                        clipData.clipNameCustom = EditorGUILayout.TextField("clip name:", clipData.clipNameCustom, GUILayout.ExpandWidth(true));

                        // FPS
                        EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
                        {
                            EditorGUI.BeginChangeCheck();
                            {
                                clipData.fpsEnum = (FPS)EditorGUILayout.EnumPopup("fps:", clipData.fpsEnum, GUILayout.Width(210));

                                // show custom FPS
                                oldEnable = GUI.enabled;
                                GUI.enabled = (GUI.enabled && clipData.fpsEnum == FPS.custom) ? true : false;
                                clipData.fps = EditorGUILayout.IntField(clipData.fps, GUILayout.Width(50));
                                GUI.enabled = oldEnable;

                                if (clipData.fpsEnum != FPS.custom)
                                    clipData.fps = (int)clipData.fpsEnum;

                            }
                            if (EditorGUI.EndChangeCheck()) clipData.updateFPS();
                        }
                        EditorGUILayout.EndHorizontal();

                        // LOOP
                        clipData.loop = EditorGUILayout.Toggle("loop:", clipData.loop);

                        // PIVOT ALIGNMENT
                        EditorGUILayout.BeginHorizontal();
                        {
                            oldEnable = GUI.enabled;

                            clipData.spriteAlignment = (SpriteAlignment)EditorGUILayout.EnumPopup("pivot:", clipData.spriteAlignment, GUILayout.Width(250));
                            GUI.enabled = (clipData.spriteAlignment == SpriteAlignment.Custom);
                            bool oldWideMode = EditorGUIUtility.wideMode;
                            EditorGUIUtility.wideMode = true; // for inline GUI field
                            clipData.pivot = EditorGUILayout.Vector2Field("", clipData.pivot, GUILayout.Width(150));
                            EditorGUIUtility.wideMode = oldWideMode;

                            GUI.enabled = oldEnable;
                        }
                        EditorGUILayout.EndHorizontal();

                        clipData.showPivot = EditorGUILayout.Toggle("show pivot", clipData.showPivot);
                        drawPivotsHandles(clipData);

                        // LEGACY
                        clipData.legacy = EditorGUILayout.Toggle("legacy:", clipData.legacy);

                        // PATH TO SAVE CLIP
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("save clip in folder:", clipData.savePath);

                            if (GUILayout.Button("change path", GUILayout.ExpandWidth(false)))
                            {
                                clipData.savePath = EditorUtility.OpenFolderPanel("Save clip in folder", "Assets", "");
                                if (clipData.savePath.Length == 0) clipData.savePath = "Assets";
                                else clipData.savePath = "Assets" + clipData.savePath.Substring(Application.dataPath.Length);
                                //Debug.Log(clipData.savePath);
                            }

                            if (GUILayout.Button("copy", EditorStyles.miniButtonLeft, GUILayout.ExpandWidth(false)))
                            {
                                _pathCopyBuffer = clipData.savePath;
                            }

                            if (GUILayout.Button("paste", EditorStyles.miniButtonRight, GUILayout.ExpandWidth(false)))
                            {
                                if (_pathCopyBuffer.Length == 0) _pathCopyBuffer = "Assets";
                                clipData.savePath = _pathCopyBuffer;
                            }
                        }
                        EditorGUILayout.EndHorizontal();

                        // HELP BOX IF 1 FRAME IN CLIP
                        if (clipData.sprites.Count == 1)
                        {
                            EditorGUILayout.HelpBox("This clip contains only one frame!\nTurn OFF the creation this clip if needed.", MessageType.Warning);
                        }


                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();
                GUI.enabled = true;
                GUI.backgroundColor = oldColor;

            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndScrollView();
        }

        private void drawPreviewLabelAndHisSettings()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal("box");
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("PREVIEW CLIPS", EditorStyles.boldLabel, GUILayout.ExpandWidth(false));
                EditorGUILayout.Space();

                if (GUILayout.Button("sort by anim exist", GUILayout.ExpandWidth(false)))
                {
                    if (_sortAnimExist)
                        _clipsData = _clipsData.OrderBy(key => key.Value.sprites.Count < 2).ToDictionary(keyItem => keyItem.Key, keyValue => keyValue.Value);
                    else
                        _clipsData = _clipsData.OrderBy(key => key.Value.sprites.Count > 1).ToDictionary(keyItem => keyItem.Key, keyValue => keyValue.Value);

                    _sortAnimExist = !_sortAnimExist;
                }

                if (GUILayout.Button("sort by clip name", GUILayout.ExpandWidth(false)))
                {
                    if (_sortByName)
                        _clipsData = _clipsData.OrderByDescending(key => key.Value.clipNameCustom).ToDictionary(keyItem => keyItem.Key, keyValue => keyValue.Value);
                    else
                        _clipsData = _clipsData.OrderBy(key => key.Value.clipNameCustom).ToDictionary(keyItem => keyItem.Key, keyValue => keyValue.Value);

                    _sortByName = !_sortByName;
                }

                drawSettingsButton();
            }
            EditorGUILayout.EndHorizontal();

            if (_showSettings)
            {
                EditorGUILayout.BeginHorizontal();
                drawSettings();
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Space();
        }

        private void drawPivotsHandles(ClipData clipData)
        {
            Rect spriteRect = clipData.sprites[clipData.currentFrame].rect;

            float maxSide = Mathf.Max(spriteRect.width, spriteRect.height);
            if (maxSide > _maxPreviewClipSide)
            {
                spriteRect.size *= _maxPreviewClipSide / maxSide;
            }

            spriteRect.center = clipData.spriteLastRect.center;

            Vector3 pivotPoint = new Vector3();
            float w = spriteRect.width;
            float h = spriteRect.height;
            float wHalf = w * 0.5f;
            float hHalf = h * 0.5f;

            switch (clipData.spriteAlignment)
            {
                case SpriteAlignment.Center:
                pivotPoint = spriteRect.center;
                break;
                case SpriteAlignment.TopLeft:
                pivotPoint = spriteRect.min;
                break;
                case SpriteAlignment.TopCenter:
                pivotPoint.Set(spriteRect.x + wHalf, spriteRect.y, 0);
                break;
                case SpriteAlignment.TopRight:
                pivotPoint.Set(spriteRect.x + w, spriteRect.y, 0);
                break;
                case SpriteAlignment.LeftCenter:
                pivotPoint.Set(spriteRect.x, spriteRect.y + hHalf, 0);
                break;
                case SpriteAlignment.RightCenter:
                pivotPoint.Set(spriteRect.x + w, spriteRect.y + spriteRect.height * 0.5f, 0);
                break;
                case SpriteAlignment.BottomLeft:
                pivotPoint.Set(spriteRect.x, spriteRect.y + h, 0);
                break;
                case SpriteAlignment.BottomCenter:
                pivotPoint.Set(spriteRect.x + wHalf, spriteRect.y + h, 0);
                break;
                case SpriteAlignment.BottomRight:
                pivotPoint = spriteRect.max;
                break;
                case SpriteAlignment.Custom:
                pivotPoint = spriteRect.position + new Vector2(clipData.pivot.x * spriteRect.width, (clipData.pivot.y * -spriteRect.height) + spriteRect.height);
                break;
                default:
                break;
            }
            Handles.BeginGUI();
            {
                Vector3 p_1 = new Vector3(spriteRect.x, spriteRect.y);
                Vector3 p_2 = new Vector3(spriteRect.x + spriteRect.width, spriteRect.y);
                Vector3 p_3 = new Vector3(spriteRect.x + spriteRect.width, spriteRect.y + spriteRect.height);
                Vector3 p_4 = new Vector3(spriteRect.x, spriteRect.y + spriteRect.height);

                Handles.DrawAAPolyLine(p_1, p_2, p_3, p_4, p_1);

                if (clipData.isCreateClip && clipData.showPivot)
                {
                    Color oldColor = Handles.color;
                    Handles.color = _pivotColor;
                    Handles.DrawSolidDisc(pivotPoint, Vector3.forward, 3);
                    Handles.color = oldColor;
                }
            }
            Handles.EndGUI();

        }

        private void resetAll()
        {
            _xml = null;
            _spriteSheet = null;
            _pathCopyBuffer = "";
            _showSettings = false;
            _somePivotIsChanged = false;
            _clipsData.Clear();
        }

        private void cutSpriteSheet()
        {
            float progressBarCounter = 0;
            EditorUtility.DisplayProgressBar("Cut sprite sheet", "Reading XML", 0f);

            List<SpriteMetaData> spritesMetaData = new List<SpriteMetaData>();
            IEnumerable<XElement> allSubTextures = XElement.Load(AssetDatabase.GetAssetPath(_xml)).Elements("SubTexture");


            // create sprites metaData from xml


            foreach (XElement subTexture in allSubTextures)
            {
                Rect rect = new Rect();
                rect.width = (int)subTexture.Attribute("width");
                rect.height = (int)subTexture.Attribute("height");
                rect.x = (int)subTexture.Attribute("x");
                rect.y = _spriteSheet.texture.height - (int)subTexture.Attribute("y") - rect.height;

                SpriteMetaData spriteMetaData = new SpriteMetaData();
                spriteMetaData.rect = rect;
                spriteMetaData.name = subTexture.Attribute("name").Value;
                spriteMetaData.name = Path.GetFileNameWithoutExtension(spriteMetaData.name).Replace(" ", "");
                //spriteMetaData.alignment = (int)SpriteAlignment.Custom;
                //spriteMetaData.pivot = new Vector2(0.3f, 0.7f);

                spritesMetaData.Add(spriteMetaData);

                progressBarCounter++;
                EditorUtility.DisplayProgressBar("Cut sprite sheet", "Reading XML", progressBarCounter / allSubTextures.Count());
            }


            // save cuted sprite
            EditorUtility.DisplayProgressBar("Cut sprite sheet", "Create sprite sheet", 0f);

            string spriteSheetPath = AssetDatabase.GetAssetPath(_spriteSheet);
            TextureImporter textureImporter = AssetImporter.GetAtPath(spriteSheetPath) as TextureImporter;
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.spritesheet = spritesMetaData.ToArray();

            EditorUtility.DisplayProgressBar("Cut sprite sheet", "Apply settings", 0.3f);


            TextureImporterSettings textureImporterSettings = new TextureImporterSettings();
            textureImporter.ReadTextureSettings(textureImporterSettings);

            textureImporterSettings.spriteMode = (int)SpriteImportMode.Multiple;
            textureImporterSettings.readable = true;
            textureImporterSettings.mipmapEnabled = false;

            EditorUtility.DisplayProgressBar("Cut sprite sheet", "Set settings", 0.5f);

            textureImporter.SetTextureSettings(textureImporterSettings);

            EditorUtility.DisplayProgressBar("Cut sprite sheet", "Save sprite sheet", 0.75f);

            AssetDatabase.ImportAsset(spriteSheetPath, ImportAssetOptions.ForceUpdate);

            //update link after import
            _spriteSheet = AssetDatabase.LoadAssetAtPath<Sprite>(spriteSheetPath);
            
            EditorUtility.DisplayProgressBar("Cut sprite sheet", "Finish created", 0.99f);

            // create clips data
            createClipData();

        }

        private void createClipData(bool checkReadableTexture = false)
        {
            _clipsData.Clear();
            string spriteSheetPath = AssetDatabase.GetAssetPath(_spriteSheet);

            // make texture is readable if it is false
            if (checkReadableTexture)
            {
                EditorUtility.DisplayProgressBar("Create preview", "Make texture is readable", 0f);
                TextureImporter textureImporter = AssetImporter.GetAtPath(spriteSheetPath) as TextureImporter;
                TextureImporterSettings textureImporterSettings = new TextureImporterSettings();
                textureImporter.ReadTextureSettings(textureImporterSettings);
                if (!textureImporterSettings.readable)
                {
                    textureImporterSettings.readable = true;
                    textureImporter.SetTextureSettings(textureImporterSettings);
                    AssetDatabase.ImportAsset(spriteSheetPath, ImportAssetOptions.ForceUpdate);
                }
            }


            Sprite[] allSprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheetPath).OfType<Sprite>().ToArray();

            List<SpriteMetaData> metaDatas = (AssetImporter.GetAtPath(spriteSheetPath) as TextureImporter).spritesheet.ToList();

            float progressBarCounter = 0;
            EditorUtility.DisplayProgressBar("Create preview", "Create clips preview", 0f);


            foreach (Sprite sprite in allSprites)
            {
                string shorSpriteName = extractShortName(sprite.name); ;

                if (!_clipsData.ContainsKey(shorSpriteName))
                {
                    ClipData clipData = new ClipData(shorSpriteName);
                    SpriteMetaData meta = metaDatas.Find(m => m.name == sprite.name);
                    clipData.spriteAlignment = (SpriteAlignment)meta.alignment;
                    clipData.pivot = meta.pivot;
                    _clipsData[shorSpriteName] = clipData;
                }

                _clipsData[shorSpriteName].addSprite(sprite);

                progressBarCounter++;
                EditorUtility.DisplayProgressBar("Create preview", "Create clips preview", progressBarCounter / allSprites.Length);
            }

            EditorUtility.ClearProgressBar();
        }


        private void rebuildSpritePivotsInSpriteSheet()
        {
            if (!_somePivotIsChanged) return;

            float progressBarCounter = 0;
            EditorUtility.DisplayProgressBar("Rebuild pivots in sprite sheet", "Reading sprite sheet", 0f);

            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(_spriteSheet), ImportAssetOptions.ForceUpdate);

            TextureImporter textureImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(_spriteSheet)) as TextureImporter;
            textureImporter.isReadable = true;
            List<SpriteMetaData> metas = new List<SpriteMetaData>();
            List<ClipData> allClips = _clipsData.Values.ToList();
            string shortName;
            for (int i = 0; i < textureImporter.spritesheet.Length; i++)
            {
                SpriteMetaData spriteMetaData = textureImporter.spritesheet[i];
                shortName = extractShortName(spriteMetaData.name);

                ClipData currentClip = allClips.Find(clip => clip.clipName == shortName);

                spriteMetaData.alignment = (int)currentClip.spriteAlignment;
                if (currentClip.spriteAlignment == SpriteAlignment.Custom)
                {
                    spriteMetaData.pivot = currentClip.pivot;
                }
                metas.Add(spriteMetaData);

                progressBarCounter++;
                EditorUtility.DisplayProgressBar("Rebuild pivots in sprite sheet",
                                                "Change pivot " + progressBarCounter + "/" + textureImporter.spritesheet.Length,
                                                progressBarCounter / textureImporter.spritesheet.Length);

            }
            textureImporter.spritesheet = metas.ToArray();
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(_spriteSheet), ImportAssetOptions.ForceUpdate);

            EditorUtility.ClearProgressBar();

            _somePivotIsChanged = false;
        }

        private void saveClips(bool onlyWithAnimation = false)
        {
            rebuildSpritePivotsInSpriteSheet();

            float progressBarCounter = 0;
            EditorUtility.DisplayProgressBar("Save clips",
                                            "Saved clips" + progressBarCounter + "/" + _clipsData.Count,
                                            progressBarCounter / _clipsData.Count);

            foreach (ClipData clipData in _clipsData.Values)
            {
                if (!clipData.isCreateClip) continue;
                if (onlyWithAnimation && clipData.sprites.Count < 2) continue;

                EditorCurveBinding curve = new EditorCurveBinding();
                curve.path = "";
                curve.type = typeof(SpriteRenderer);
                curve.propertyName = "m_Sprite";

                ObjectReferenceKeyframe[] keys = new ObjectReferenceKeyframe[clipData.sprites.Count];

                for (int i = 0; i < keys.Length; i++)
                {
                    keys[i] = new ObjectReferenceKeyframe();
                    keys[i].time = (float)i / clipData.fps;
                    keys[i].value = clipData.sprites[i];
                }

                AnimationClip clip = new AnimationClip();
                clip.name = clipData.clipNameCustom;
                clip.frameRate = clipData.fps;
                clip.legacy = clipData.legacy;

                AnimationClipSettings clipSetting = new AnimationClipSettings();
                clipSetting.loopTime = clipData.loop;

                AnimationUtility.SetAnimationClipSettings(clip, clipSetting);
                AnimationUtility.SetObjectReferenceCurve(clip, curve, keys);

                AssetDatabase.CreateAsset(clip, clipData.savePath + "/" + clipData.clipNameCustom + ".anim");

                progressBarCounter++;
                EditorUtility.DisplayProgressBar("Save clips",
                                            "Saved clips " + progressBarCounter + "/" + _clipsData.Count,
                                            progressBarCounter / _clipsData.Count);
            }
            AssetDatabase.Refresh();

            EditorUtility.ClearProgressBar();
        }

        private void PIVOT_CHANGE_Handler()
        {
            //Debug.Log("pivot is changed");
            _somePivotIsChanged = true;
        }

        private void drawHelpButton()
        {
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("ShoeBox page", GUILayout.ExpandWidth(false)))
            {
                Application.OpenURL("http://renderhjs.net/shoebox/");
            }
            EditorGUILayout.EndHorizontal();
        }

        // helpers
        private string extractShortName(string nameWithoutExtention)
        {
            int lastUnd = nameWithoutExtention.LastIndexOf('_');
            if (lastUnd == -1)
            {
                //Debug.Log(_inputName + " не содержит символа _");
                return nameWithoutExtention;
            }

            if (lastUnd == nameWithoutExtention.Length - 1)
            {
                //Debug.Log("символ _ последний в имени");
                return nameWithoutExtention.Substring(0, lastUnd);
            }
            else
            {
                string after_ = nameWithoutExtention.Substring(lastUnd + 1);

                //Debug.Log("после_: " + after_);
                int r;
                bool isDigit = int.TryParse(after_, out r);
                //Debug.Log("конвертация в число: " + isDigit);
                if (isDigit) return nameWithoutExtention.Substring(0, lastUnd);
            }

            return nameWithoutExtention;
        }

        private void makeSpaceTab(int spacePixels)
        {
            EditorGUILayout.LabelField("", GUILayout.Width(spacePixels));
        }

        private void setGUIEnable(bool isEnable)
        {
            if (_countGUIEnabled) Debug.LogWarning("setGUIEnable not balanced");
            _countGUIEnabled = true;
            _oldGUIEnable = GUI.enabled;
            GUI.enabled = isEnable;
        }

        private void restoreGUIEnabled()
        {
            GUI.enabled = _oldGUIEnable;
            _countGUIEnabled = false;
        }

        private void showDebugInfo()
        {
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.LabelField("DEBUG INFO", EditorStyles.whiteBoldLabel);
                EditorGUILayout.ObjectField("_spriteSheet", _spriteSheet, typeof(Texture2D), false);
                EditorGUILayout.ObjectField("_xml", _xml, typeof(TextAsset), false);
                EditorGUILayout.Space();
                EditorGUILayout.Toggle("_clipData is null:", _clipsData == null);
                EditorGUILayout.IntField("_clipData.Values.Count", _clipsData.Values.Count);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("_pathCopyBuffer", _pathCopyBuffer);
                EditorGUILayout.Toggle("_somePivotIsChanged", _somePivotIsChanged);
            }
            EditorGUILayout.EndVertical();
        }

    }

    class ClipData
    {
        internal string clipName;
        internal string clipNameCustom;
        internal bool loop = true;
        internal bool legacy;
        internal bool isCreateClip = true;

        internal int fps = 30;
        internal FPS fpsEnum = FPS._30;

        internal string savePath = "Assets";

        // pivot data
        private SpriteAlignment _spriteAlignment = SpriteAlignment.Center;
        internal SpriteAlignment spriteAlignment
        {
            get { return _spriteAlignment; }
            set
            {
                if (spriteAlignment != value && PIVOT_CHANGED != null)
                {
                    PIVOT_CHANGED();
                }
                _spriteAlignment = value;
            }
        }

        private Vector2 _pivot = Vector2.zero;
        internal Vector2 pivot
        {
            get { return _pivot; }
            set
            {
                if (pivot != value && PIVOT_CHANGED != null)
                {
                    PIVOT_CHANGED();
                }
                _pivot = value;
            }
        }

        internal static Action PIVOT_CHANGED;
        // end pivot data

        internal bool showPivot;
        // for draw pivot
        internal Rect spriteLastRect;

        internal List<Sprite> sprites;
        internal List<Texture2D> textures;

        float _nextTime;
        int _frame = 0;

        internal int currentFrame { get { return _frame; } }


        public ClipData(string clipName)
        {
            this.clipName = clipName;
            clipNameCustom = clipName;
            sprites = new List<Sprite>();
            textures = new List<Texture2D>();
            _nextTime = Time.realtimeSinceStartup + (1f / fps);
        }

        public void addSprite(Sprite sprite)
        {
            sprites.Add(sprite);

            Rect spriteRect = sprite.rect;
            Color[] colors = sprite.texture.GetPixels((int)spriteRect.x, (int)spriteRect.y, (int)spriteRect.width, (int)spriteRect.height);
            Texture2D texture = new Texture2D((int)spriteRect.width, (int)spriteRect.height);
            texture.SetPixels(colors);
            texture.Apply();
            textures.Add(texture);
        }

        public void updateFrame()
        {
            if (!isCreateClip || textures == null || textures.Count == 0) return;

            if (Time.realtimeSinceStartup >= _nextTime)
            {
                _frame++;
                if (_frame >= textures.Count) _frame = 0;
                _nextTime = Time.realtimeSinceStartup + (1f / fps);
            }
        }

        public void updateFPS()
        {
            _nextTime = Time.realtimeSinceStartup + (1f / fps);
        }

    }

    enum FPS
    {
        _10 = 10,
        _12 = 12,
        _15 = 15,
        _20 = 20,
        _24 = 24,
        _25 = 25,
        _30 = 30,
        _60 = 60,
        custom
    }
}



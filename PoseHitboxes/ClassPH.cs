using Il2CppRUMBLE.Tutorial.MoveLearning;
using MelonLoader;
using RumbleModdingAPI;
using RumbleModdingAPI.RMAPI;
using System.Collections;

//using static RumbleModdingAPI.Calls;
using UnityEngine;
//using static RUMBLE.Players.Subsystems.PlayerVR;
//using static RumbleModdingAPI.Calls.GameObjects.Gym.Logic.HeinhouserProducts.MoveLearning.MoveLearnSelector;
//using static RumbleModdingAPI.Calls.GameObjects.Gym.Logic.HeinhouserProducts.MoveLearning;
//using static RumbleModdingAPI.Calls.GameObjects.Gym.Logic.HeinhouserProducts;
//using MelonLoader.TinyJSON;
//using static RumbleModdingAPI.Calls.GameObjects.Park.Scene.Park.SubStaticGroup.Station.Gondola.Step;


namespace PoseHitboxes
{
    public class ClassPH : MelonMod
    {
        private string currentScene = "Loader";
        private bool sceneChanged = false;
        private bool loaded = false;
        PoseGhost poseGhost;
        Material ghosty;
        Material redy;
        Material glassy;
        string currentPoseName = "";
        int delayCycle = 0;
        //GameObject LCube;
        //GameObject RCube;
        GameObject LHitbox;
        GameObject RHitbox;
        GameObject LRotate;
        GameObject RRotate;
        GameObject LRotateLimit;
        GameObject RRotateLimit;
        GameObject LHandRef;
        GameObject RHandRef;
        ////GameObject HBRef;
        ////GameObject HBRefHead;
        float deg2rad = 3.14159f / 180f;
        float LThresh = 0f;
        float RThresh = 0f;
        Color red8 = new Color(1.0f, 0.0f, 0.0f, 0.8f);
        Color green8 = new Color(0.0f, 1.0f, 0.0f, 0.8f);
        Color blue8 = new Color(0.0f, 0.0f, 1.0f, 0.8f);
        Color black8 = new Color(0.0f, 0.0f, 0.0f, 0.8f);
        Color red1 = new Color(1.0f, 0.0f, 0.0f, 0.1f);
        Color green1 = new Color(0.0f, 1.0f, 0.0f, 0.1f);
        Color blue1 = new Color(0.0f, 0.0f, 1.0f, 0.1f);
        object coroutine = null;

        //creates a child set of WFParent with a given material and color in the shape of a 1x1x1 wireframe cube made of primative cubes. 
        private void BuildWireframeBox(GameObject WFParent, Material WFMat, Color WFColor)
        {
            GameObject[,] spokes = new GameObject[3, 4];
            int[,] dArray = { { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 } };
            for (int xyz = 0; xyz < 3; xyz++)
            {
                for (int edges = 0; edges < 4; edges++)
                {
                    spokes[xyz, edges] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    spokes[xyz, edges].transform.parent = WFParent.transform;
                    spokes[xyz, edges].GetComponent<Renderer>().material = WFMat;
                    spokes[xyz, edges].GetComponent<Renderer>().material.color = WFColor;
                    switch (xyz)
                    {
                        case 0:
                            spokes[xyz, edges].transform.localScale = new Vector3(1f, 0.003f, 0.003f);
                            spokes[xyz, edges].transform.localPosition = new Vector3(0f, 0.5f * dArray[edges, 0], 0.5f * dArray[edges, 1]);
                            break;
                        case 1:
                            spokes[xyz, edges].transform.localScale = new Vector3(0.003f, 1f, 0.003f);
                            spokes[xyz, edges].transform.localPosition = new Vector3(0.5f * dArray[edges, 0], 0f, 0.5f * dArray[edges, 1]);
                            break;
                        case 2:
                            spokes[xyz, edges].transform.localScale = new Vector3(0.003f, 0.003f, 1f);
                            spokes[xyz, edges].transform.localPosition = new Vector3(0.5f * dArray[edges, 0], 0.5f * dArray[edges, 1], 0f);
                            break;
                    }
                }
            }
        }


        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            currentScene = sceneName;
            sceneChanged = true;
            delayCycle = 5;
            loaded = false;
            //MelonLogger.Msg("loaded = ");
            //MelonLogger.Msg(loaded);
        }

        public override void OnFixedUpdate()
        {
            if (currentScene != "Gym") { return; }
            if (sceneChanged)
            {
                if (delayCycle > 1)
                {
                    delayCycle--; // wait 5 frames before trying to fetch anything
                    //MelonLogger.Msg(delayCycle);
                }
                else
                {
                    if (!loaded)
                    {
                        try
                        {
                            //MelonLogger.Msg("loading items");
                            poseGhost = GameObjects.Gym.INTERACTABLES.PoseGhost.Ghost.GetGameObject().GetComponentInChildren<PoseGhost>();//<PoseGhost>();//GameObject.Find("--------------LOGIC--------------/Heinhouser products/MoveLearning/Ghost").GetComponent<PoseGhost>();
                            ghosty = poseGhost.ActiveInstance.BodyRenderer.transform.parent.GetChild(2).GetComponent<Renderer>().material;
                            //redy = Calls.GameObjects.Gym.SCENE.GymProduction.Mainstaticgroup.Gymarena.GetGameObject().transform.GetChild(0).GetComponent<Renderer>().material;
                            redy = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
                            ////HBRef = GameObject.Find("Player Controller(Clone)/Visuals/RIG");///Bone_Shoulderblade_L/Bone_Shoulder_L/Bone_Lowerarm_L/Bone_Hand_L");
                            ////HBRefHead = GameObject.Find("Player Controller(Clone)/Visuals/RIG/Bone_Pelvis/Bone_Spine/Bone_Chest/Bone_Neck/Bone_Head");
                            LHandRef = GameObject.Find("Player Controller(Clone)/Visuals/Skelington/Bone_Pelvis/Bone_Spine_A/Bone_Chest/Bone_Shoulderblade_L/Bone_Shoulder_L/Bone_Lowerarm_L/Bone_HandAlpha_L");
                            RHandRef = GameObject.Find("Player Controller(Clone)/Visuals/Skelington/Bone_Pelvis/Bone_Spine_A/Bone_Chest/Bone_Shoulderblade_R/Bone_Shoulder_R/Bone_Lowerarm_R/Bone_HandAlpha_R");
                            //--------------LOGIC-------------- / Heinhouser products / MoveLearning / MoveLearnSelector / TotemPedistalCompact /
                            loaded = true;
                            currentPoseName = "";
                            //MelonLogger.Msg("items loaded");
                        }
                        catch { return; }
                    }
                    sceneChanged = false;
                }
            }
            if (loaded)
            {
                //MelonLogger.Msg("chkpt 1");
                //MelonLogger.Msg("loaded start");
                //if pose switched 
                try
                {
                    if (currentPoseName != poseGhost.CurrentPoseSet.configurations[poseGhost.currentVisualPoseIndex].Pose.name)
                    {
                        //MelonLogger.Msg("chkpt 2");
                        if (poseGhost.ActiveInstance.BodyRenderer.transform.parent.GetChild(0).childCount > 0)
                        {
                            //MelonLogger.Msg("chkpt 3");
                            // MelonLogger.Msg("Destroying Existing");
                            while (poseGhost.ActiveInstance.BodyRenderer.transform.parent.GetChild(0).childCount > 0 || poseGhost.ActiveInstance.BodyRenderer.transform.parent.GetChild(1).childCount > 0)
                            {
                                try
                                {
                                    GameObject.DestroyImmediate(poseGhost.ActiveInstance.BodyRenderer.transform.parent.GetChild(0).GetChild(0).gameObject);
                                    GameObject.DestroyImmediate(poseGhost.ActiveInstance.BodyRenderer.transform.parent.GetChild(1).GetChild(0).gameObject);
                                    GameObject.DestroyImmediate(LRotate.gameObject);
                                    GameObject.DestroyImmediate(RRotate.gameObject);
                                    //MelonLogger.Msg("objects destroyed");
                                }
                                catch
                                { return; }
                            }
                        }
                        if (coroutine != null)
                        {
                            MelonCoroutines.Stop(coroutine);
                        }
                        coroutine = MelonCoroutines.Start(CreateHitboxes());
                    }
                }

                catch
                {
                    return;
                }
            }
            try
            {
                if (LRotate != null)//// && LCube != null)
                {
                    //MelonLogger.Msg("updateAngle");
                    LRotate.transform.rotation = LHandRef.GetComponent<Transform>().rotation;
                    RRotate.transform.rotation = RHandRef.GetComponent<Transform>().rotation;
                    ////LCube.transform.position = HBRefHead.transform.position; //player hitboxes
                    ////RCube.transform.position = poseGhost.rightHand.position; //player hitboxes
                    //LRotate.transform.position = LHandRef.GetComponent<Transform>().position + new Vector3(0.0f, 0.2f, 0.0f);
                    //RRotate.transform.position = RHandRef.GetComponent<Transform>().position + new Vector3(0.0f, 0.2f, 0.0f);
                    //LRotateLimit.transform.position = LHandRef.GetComponent<Transform>().position + new Vector3(0.0f, 0.2f, 0.0f);
                    //RRotateLimit.transform.position = RHandRef.GetComponent<Transform>().position + new Vector3(0.0f, 0.2f, 0.0f);
                    //LRotate.transform.localRotation = LHandRef.GetComponent<Transform>().localRotation;
                    //RRotate.transform.localRotation = RHandRef.GetComponent<Transform>().localRotation;
                }
            }
            catch { return; }
        }

        IEnumerator CreateHitboxes()
        {

            currentPoseName = poseGhost.CurrentPoseSet.configurations[poseGhost.currentVisualPoseIndex].Pose.name;
            while (poseGhost.currentPoseLerp < 0.5f) { yield return null; }

            //LCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //RCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //MelonLogger.Msg("cubes start0");
            GameObject LCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject RCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //MelonLogger.Msg("cubes start1");
            LCube.transform.parent = poseGhost.ActiveInstance.BodyRenderer.transform.parent.GetChild(0);
            RCube.transform.parent = poseGhost.ActiveInstance.BodyRenderer.transform.parent.GetChild(1);
            //MelonLogger.Msg("cubes start2");
            LCube.transform.position = poseGhost.ActiveInstance.LeftHand.position;//showCurrentPoseData.leftControllerCondition.DesiredPose.position;
            RCube.transform.position = poseGhost.ActiveInstance.RightHand.position;
            //MelonLogger.Msg("cubes made");

            BuildWireframeBox(LCube.gameObject, redy, black8);
            BuildWireframeBox(RCube.gameObject, redy, black8);
            //MelonLogger.Msg("wires made");
            ////LCube.transform.parent = HBRef.transform; // poseGhost.transform.GetChild(0).GetChild(0);
            ////RCube.transform.parent = HBRef.transform; //poseGhost.transform.GetChild(0).GetChild(1);
            ////LCube.transform.localPosition = poseGhost.leftHand.localPosition + new Vector3(0.5f, 0.5f, 0f);
            ////RCube.transform.localPosition = poseGhost.rightHand.localPosition + new Vector3(0.5f, 0.5f, 0f);
            ////LCube.transform.RotateAround(HBRef.transform.position, Vector3.forward, 180f); //flip it over, not sure why it is upside down
            ////RCube.transform.RotateAround(HBRef.transform.position, Vector3.forward, 180f); //flip it over, not sure why it is upside down

            LCube.GetComponent<Renderer>().enabled = false;
            RCube.GetComponent<Renderer>().enabled = false;
            //poseGhost.ActiveInstance.BodyRenderer.transform.parent.GetChild(0).GetComponent<Renderer>().material = redy;
            //poseGhost.ActiveInstance.BodyRenderer.transform.parent.GetChild(1).GetComponent<Renderer>().material = redy;


            //setup gyro objects and details
            LRotate = GameObject.CreatePrimitive(PrimitiveType.Cube);
            RRotate = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject LYBar = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject LZBar = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject RYBar = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject RZBar = GameObject.CreatePrimitive(PrimitiveType.Cube);

            LRotate.transform.parent = poseGhost.ActiveInstance.BodyRenderer.transform.parent.GetChild(0);
            RRotate.transform.parent = poseGhost.ActiveInstance.BodyRenderer.transform.parent.GetChild(1);

            LRotate.GetComponent<Renderer>().material = redy;
            RRotate.GetComponent<Renderer>().material = redy;
            LRotate.transform.localScale = new Vector3(0.003f, 0.003f, 0.003f);
            RRotate.transform.localScale = new Vector3(0.003f, 0.003f, 0.003f);

            LYBar.transform.parent = LRotate.transform;
            LZBar.transform.parent = LRotate.transform;
            RYBar.transform.parent = RRotate.transform;
            RZBar.transform.parent = RRotate.transform;

            LYBar.transform.localScale = new Vector3(1f, 25f, 1f);
            LZBar.transform.localScale = new Vector3(1f, 1f, 25f);
            RYBar.transform.localScale = new Vector3(1f, 25f, 1f);
            RZBar.transform.localScale = new Vector3(1f, 1f, 25f);

            LYBar.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
            LZBar.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
            RYBar.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
            RZBar.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");

            LYBar.GetComponent<Renderer>().material.color = green8;
            LZBar.GetComponent<Renderer>().material.color = blue8;
            RYBar.GetComponent<Renderer>().material.color = green8;
            RZBar.GetComponent<Renderer>().material.color = blue8;

            LRotate.transform.position = poseGhost.ActiveInstance.LeftHand.position + new Vector3(-1.0f, 0.00f, -1.0f);
            RRotate.transform.position = poseGhost.ActiveInstance.RightHand.position + new Vector3(-1.0f, 0.00f, -1.0f);
            //MelonLogger.Msg("gyros made");
            //generate reference cylinders

            LRotateLimit = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            RRotateLimit = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            GameObject LYBarLimit = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            GameObject LZBarLimit = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            GameObject RYBarLimit = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            GameObject RZBarLimit = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

            //calculate trig for degree limits


            if (poseGhost.ActiveInstance.LeftHand.rotation.eulerAngles.x == poseGhost.CurrentPoseSet.configurations[poseGhost.currentVisualPoseIndex].Pose.leftControllerCondition.DesiredPose.rotation.eulerAngles.x) // if the hands are not mirrored, use the normal map
            {
                LThresh = poseGhost.CurrentPoseSet.configurations[poseGhost.currentVisualPoseIndex].Pose.LeftControllerCondition.RotationTreshold;
                RThresh = poseGhost.CurrentPoseSet.configurations[poseGhost.currentVisualPoseIndex].Pose.RightControllerCondition.RotationTreshold;
                LCube.transform.localScale = new Vector3(poseGhost.CurrentPoseSet.configurations[poseGhost.currentVisualPoseIndex].Pose.leftControllerCondition.PositionTreshold, poseGhost.CurrentPoseSet.configurations[poseGhost.currentVisualPoseIndex].Pose.leftControllerCondition.PositionTreshold, poseGhost.CurrentPoseSet.configurations[poseGhost.currentVisualPoseIndex].Pose.leftControllerCondition.PositionTreshold);
                RCube.transform.localScale = new Vector3(poseGhost.CurrentPoseSet.configurations[poseGhost.currentVisualPoseIndex].Pose.RightControllerCondition.PositionTreshold, poseGhost.CurrentPoseSet.configurations[poseGhost.currentVisualPoseIndex].Pose.RightControllerCondition.PositionTreshold, poseGhost.CurrentPoseSet.configurations[poseGhost.currentVisualPoseIndex].Pose.RightControllerCondition.PositionTreshold);

                // MelonLogger.Msg("right hand mode");
            }
            else // they need to be swapped
            {
                RThresh = poseGhost.CurrentPoseSet.configurations[poseGhost.currentVisualPoseIndex].Pose.LeftControllerCondition.RotationTreshold;
                LThresh = poseGhost.CurrentPoseSet.configurations[poseGhost.currentVisualPoseIndex].Pose.RightControllerCondition.RotationTreshold;
                RCube.transform.localScale = new Vector3(poseGhost.CurrentPoseSet.configurations[poseGhost.currentVisualPoseIndex].Pose.leftControllerCondition.PositionTreshold, poseGhost.CurrentPoseSet.configurations[poseGhost.currentVisualPoseIndex].Pose.leftControllerCondition.PositionTreshold, poseGhost.CurrentPoseSet.configurations[poseGhost.currentVisualPoseIndex].Pose.leftControllerCondition.PositionTreshold);
                LCube.transform.localScale = new Vector3(poseGhost.CurrentPoseSet.configurations[poseGhost.currentVisualPoseIndex].Pose.RightControllerCondition.PositionTreshold, poseGhost.CurrentPoseSet.configurations[poseGhost.currentVisualPoseIndex].Pose.RightControllerCondition.PositionTreshold, poseGhost.CurrentPoseSet.configurations[poseGhost.currentVisualPoseIndex].Pose.RightControllerCondition.PositionTreshold);

                //MelonLogger.Msg("left hand mode");
            }

            float LRotationLimit = ((1 - LThresh) * 3.14159f);
            float RRotationLimit = ((1 - RThresh) * 3.14159f);

            LRotateLimit.transform.parent = poseGhost.ActiveInstance.BodyRenderer.transform.parent.GetChild(0);
            RRotateLimit.transform.parent = poseGhost.ActiveInstance.BodyRenderer.transform.parent.GetChild(1);

            LRotateLimit.GetComponent<Renderer>().material = redy;
            RRotateLimit.GetComponent<Renderer>().material = redy;
            LRotateLimit.transform.localScale = new Vector3(0.03f * LThresh, 0.03f * LThresh, 0.03f * LThresh);
            RRotateLimit.transform.localScale = new Vector3(0.03f * RThresh, 0.03f * RThresh, 0.03f * RThresh);

            LYBarLimit.transform.parent = LRotateLimit.transform;
            LZBarLimit.transform.parent = LRotateLimit.transform;
            RYBarLimit.transform.parent = RRotateLimit.transform;
            RZBarLimit.transform.parent = RRotateLimit.transform;

            LYBarLimit.transform.localScale = new Vector3(LRotationLimit, 0.6f, LRotationLimit);
            LZBarLimit.transform.localScale = new Vector3(LRotationLimit, 0.6f, LRotationLimit);
            RYBarLimit.transform.localScale = new Vector3(RRotationLimit, 0.6f, RRotationLimit);
            RZBarLimit.transform.localScale = new Vector3(RRotationLimit, 0.6f, RRotationLimit);

            LYBarLimit.transform.Rotate(new Vector3(0f, 90f, 0f), Space.Self);
            LZBarLimit.transform.Rotate(new Vector3(90f, 0f, 0f), Space.Self);
            RYBarLimit.transform.Rotate(new Vector3(0f, 90f, 0f), Space.Self);
            RZBarLimit.transform.Rotate(new Vector3(90f, 0f, 0f), Space.Self);

            LYBarLimit.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
            LZBarLimit.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
            RYBarLimit.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
            RZBarLimit.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");

            LYBarLimit.GetComponent<Renderer>().material.color = green1;
            LZBarLimit.GetComponent<Renderer>().material.color = blue1;
            RYBarLimit.GetComponent<Renderer>().material.color = green1;
            RZBarLimit.GetComponent<Renderer>().material.color = blue1;

            LRotateLimit.transform.position = poseGhost.ActiveInstance.LeftHand.position + new Vector3(-0.3f, 0.15f, -0.3f);
            RRotateLimit.transform.position = poseGhost.ActiveInstance.RightHand.position + new Vector3(-0.3f, 0.15f, -0.3f);
            LRotate.transform.position = poseGhost.ActiveInstance.LeftHand.position + new Vector3(-0.3f, 0.15f, -0.3f);
            RRotate.transform.position = poseGhost.ActiveInstance.RightHand.position + new Vector3(-0.3f, 0.15f, -0.3f);
            //if (poseGhost.ActiveInstance.LeftHand.rotation.eulerAngles.x == poseGhost.CurrentPoseSet.configurations[poseGhost.currentVisualPoseIndex].Pose.leftControllerCondition.DesiredPose.rotation.eulerAngles.x) // if the hands are not mirrored, use the normal map
           // {
                LCube.transform.localRotation = poseGhost.CurrentPoseSet.configurations[poseGhost.currentVisualPoseIndex].Pose.leftControllerCondition.DesiredPose.rotation;
                RCube.transform.localRotation = poseGhost.CurrentPoseSet.configurations[poseGhost.currentVisualPoseIndex].Pose.rightControllerCondition.DesiredPose.rotation;
                //MelonLogger.Msg("left to left, right to right");
          //  }
          //  else // they need to be swapped
          //  {
          //      RCube.transform.localRotation = poseGhost.CurrentPoseSet.configurations[poseGhost.currentVisualPoseIndex].Pose.leftControllerCondition.DesiredPose.rotation;
          //      LCube.transform.localRotation = poseGhost.CurrentPoseSet.configurations[poseGhost.currentVisualPoseIndex].Pose.rightControllerCondition.DesiredPose.rotation;
          //      MelonLogger.Msg("swapped");
          //  }
            LRotateLimit.transform.rotation = LCube.transform.rotation;
            RRotateLimit.transform.rotation = RCube.transform.rotation;
            //MelonLogger.Msg("gyros done");
            yield break;
        }
    }
}
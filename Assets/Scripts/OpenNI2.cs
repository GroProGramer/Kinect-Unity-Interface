using UnityEngine;
using System.Collections;
using xn;
using xnv;
using MyLibMono;
using System;

public class OpenNI2 : MonoBehaviour
{
	public Transform leftHand;
	public Transform rightHand;
	public Transform head;
	public float scale = 100.0f;
	public Vector3 bias = new Vector3 (0, 0, 0);
	public bool drawSkeleton = true;

	public Material rgbMaterial;

	private string XML_FILE = ".//OpenNI.xml";

	private Context context;
	public DepthGenerator depth;
	private ImageGenerator imageGenerator;
	private UserGenerator userGenerator;
	private SkeletonCapability skeletonCapability;
	private PoseDetectionCapability poseDetectionCapability;
	private HandsGenerator handsGenerator;
	private GestureGenerator gestureGenerator;
	private string calibPose;
	private bool shouldRun;

	private Transform mainUser;
	private Transform center;
	private Transform leftArm;
	private Transform leftLeg;
	private Transform rightArm;
	private Transform rightLeg;

	private Texture2D texture;

	public Vector3 handPoint = new Vector3 (0, 0, 0);

	void Start ()
	{
		texture = new Texture2D (640, 480, TextureFormat.RGB24, false);
		rgbMaterial.mainTextureScale = new Vector2 (0.62f, 0.93f);
		// fixes weird scaling problem
		rgbMaterial.mainTexture = texture;
		
		context = new Context (XML_FILE);
		depth = context.FindExistingNode (NodeType.Depth) as DepthGenerator;
		if (depth == null) {
			Debug.LogError ("Viewer must have a depth node!");
			return;
		}
		
		imageGenerator = context.FindExistingNode (NodeType.Image) as ImageGenerator;
		if (imageGenerator == null) {
			Debug.LogError ("Viewer must have an image generator!");
			return;
		}
		depth.GetAlternativeViewPointCap ().SetViewPoint (imageGenerator);
		
		//handsGenerator = context.FindExistingNode(NodeType.Hands) as HandsGenerator;
		//if (handsGenerator == null) {
		//	Debug.LogError("Viewer must have a hands generator!");
		//	return;
		//}
		//handsGenerator.GetAlternativeViewPointCap().SetViewPoint(imageGenerator);
		
		//gestureGenerator = context.FindExistingNode (NodeType.Gesture) as GestureGenerator;
		//if (gestureGenerator == null) {
		//	Debug.LogError ("Viewer must have a gesture node!");
		//}
		//gestureGenerator.GetAlternativeViewPointCap().SetViewPoint(imageGenerator);
		
		userGenerator = new UserGenerator (context);
		skeletonCapability = new SkeletonCapability (userGenerator);
		poseDetectionCapability = new PoseDetectionCapability (userGenerator);
		calibPose = skeletonCapability.GetCalibrationPose ();
		
		userGenerator.NewUser += new UserGenerator.NewUserHandler (userGenerator_NewUser);
		userGenerator.LostUser += new UserGenerator.LostUserHandler (userGenerator_LostUser);
		poseDetectionCapability.PoseDetected += new PoseDetectionCapability.PoseDetectedHandler (poseDetectionCapability_PoseDetected);
		skeletonCapability.CalibrationEnd += new SkeletonCapability.CalibrationEndHandler (skeletonCapability_CalibrationEnd);
		
		skeletonCapability.SetSkeletonProfile (SkeletonProfile.All);
		
		userGenerator.StartGenerating ();
		imageGenerator.StartGenerating ();
		
		//handsGenerator.HandCreate += new HandsGenerator.HandCreateHandler (hands_HandCreate);
		//handsGenerator.HandUpdate += new HandsGenerator.HandUpdateHandler (hands_HandUpdate);
		//handsGenerator.HandDestroy += new HandsGenerator.HandDestroyHandler (hands_HandDestroy);
		
		//gestureGenerator.AddGesture ("Wave");
		//gestureGenerator.GestureRecognized += new GestureGenerator.GestureRecognizedHandler (gestures_GestureRecognized);
		//gestureGenerator.StartGenerating();
		
		if (drawSkeleton) {
			GameObject o = new GameObject ("User");
			mainUser = o.transform;
			mainUser.position = new Vector3 (0, 0, 0);
			mainUser.parent = transform;
			
			GameObject obj = Instantiate (o, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
			center = obj.transform;
			//center.parent = mainUser;
			obj.name = "Center";
			createDefaultLineRenderer (center);
			
			obj = Instantiate (o, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
			leftArm = obj.transform;
			//leftArm.parent = mainUser;
			obj.name = "L Arm";
			createDefaultLineRenderer (leftArm);
			
			obj = Instantiate (o, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
			rightArm = obj.transform;
			//rightArm.parent = mainUser;
			obj.name = "R Arm";
			createDefaultLineRenderer (rightArm);
			
			obj = Instantiate (o, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
			leftLeg = obj.transform;
			//leftLeg.parent = mainUser;
			obj.name = "L Leg";
			createDefaultLineRenderer (leftLeg);
			
			obj = Instantiate (o, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
			rightLeg = obj.transform;
			//rightLeg.parent = mainUser;
			obj.name = "R Leg";
			createDefaultLineRenderer (rightLeg);
		}
	}

	void skeletonCapability_CalibrationEnd (ProductionNode node, uint id, bool success)
	{
		if (success) {
			Debug.Log ("Calibration ended successfully");
			skeletonCapability.StartTracking (id);
		} else {
			poseDetectionCapability.StartPoseDetection (calibPose, id);
		}
	}

	void poseDetectionCapability_PoseDetected (ProductionNode node, string pose, uint id)
	{
		poseDetectionCapability.StopPoseDetection (id);
		skeletonCapability.RequestCalibration (id, true);
	}

	void userGenerator_NewUser (ProductionNode node, uint id)
	{
		Debug.Log ("New User Found");
		poseDetectionCapability.StartPoseDetection (calibPose, id);
	}

	void userGenerator_LostUser (ProductionNode node, uint id)
	{
		Debug.Log ("Lost user");
	}

	void Update ()
	{
		bool doUpdate = true;
		
		context.WaitNoneUpdateAll ();
		
		// Draw the RGB data to a texture
		MyLib myLib = new MyLib ();
		IntPtr imageP = this.imageGenerator.GetImageMapPtr ();
		int texId = texture.GetNativeTextureID ();
		bool result = myLib.CamTexture (texId, 640, 480, imageP);
		Debug.Log ("Texture ID: " + texId + " Result: " + result);
		
		uint[] users = userGenerator.GetUsers ();
		foreach (uint user in users) {
			if (skeletonCapability.IsTracking (user)) {
				doUpdate = false;
				Debug.Log ("Here we go!");
				
				if (drawSkeleton) {
					LineRenderer lineRenderer = center.GetComponent (typeof(LineRenderer)) as LineRenderer;
					int i = 0;
					lineRenderer.SetVertexCount (3);
					lineRenderer.SetPosition (i++, getJointVector3 (user, SkeletonJoint.Head));
					lineRenderer.SetPosition (i++, getJointVector3 (user, SkeletonJoint.Neck));
					lineRenderer.SetPosition (i++, getJointVector3 (user, SkeletonJoint.Torso));
					//lineRenderer.SetPosition(i++, getJointVector3(user, SkeletonJoint.Waist));
					
					lineRenderer = leftArm.GetComponent (typeof(LineRenderer)) as LineRenderer;
					i = 0;
					lineRenderer.SetVertexCount (3);
					//lineRenderer.SetPosition(i++, getJointVector3(user, SkeletonJoint.LeftCollar));
					lineRenderer.SetPosition (i++, getJointVector3 (user, SkeletonJoint.LeftShoulder));
					lineRenderer.SetPosition (i++, getJointVector3 (user, SkeletonJoint.LeftElbow));
					//lineRenderer.SetPosition(i++, getJointVector3(user, SkeletonJoint.LeftWrist));
					lineRenderer.SetPosition (i++, getJointVector3 (user, SkeletonJoint.LeftHand));
					
					lineRenderer = rightArm.GetComponent (typeof(LineRenderer)) as LineRenderer;
					i = 0;
					lineRenderer.SetVertexCount (3);
					//lineRenderer.SetPosition(i++, getJointVector3(user, SkeletonJoint.RightCollar));
					lineRenderer.SetPosition (i++, getJointVector3 (user, SkeletonJoint.RightShoulder));
					lineRenderer.SetPosition (i++, getJointVector3 (user, SkeletonJoint.RightElbow));
					//lineRenderer.SetPosition(i++, getJointVector3(user, SkeletonJoint.RightWrist));
					lineRenderer.SetPosition (i++, getJointVector3 (user, SkeletonJoint.RightHand));
					
					lineRenderer = leftLeg.GetComponent (typeof(LineRenderer)) as LineRenderer;
					i = 0;
					lineRenderer.SetVertexCount (3);
					lineRenderer.SetPosition (i++, getJointVector3 (user, SkeletonJoint.LeftHip));
					lineRenderer.SetPosition (i++, getJointVector3 (user, SkeletonJoint.LeftKnee));
					//lineRenderer.SetPosition(i++, getJointVector3(user, SkeletonJoint.LeftAnkle));
					lineRenderer.SetPosition (i++, getJointVector3 (user, SkeletonJoint.LeftFoot));
					
					lineRenderer = rightLeg.GetComponent (typeof(LineRenderer)) as LineRenderer;
					i = 0;
					lineRenderer.SetVertexCount (3);
					lineRenderer.SetPosition (i++, getJointVector3 (user, SkeletonJoint.RightHip));
					lineRenderer.SetPosition (i++, getJointVector3 (user, SkeletonJoint.RightKnee));
					//lineRenderer.SetPosition(i++, getJointVector3(user, SkeletonJoint.RightAnkle));
					lineRenderer.SetPosition (i++, getJointVector3 (user, SkeletonJoint.RightFoot));
				}
				
				// Set transforms
				//Rigidbody rigidbody = leftHand.gameObject.GetComponent (typeof(Rigidbody)) as Rigidbody;
				//rigidbody.MovePosition (getJointVector3 (user, SkeletonJoint.LeftHand));
				//rigidbody = rightHand.gameObject.GetComponent (typeof(Rigidbody)) as Rigidbody;
				//rigidbody.MovePosition (getJointVector3 (user, SkeletonJoint.RightHand));
				
				if (head != null) {
					head.position = getJointVector3 (user, SkeletonJoint.Head);
				}
				
				//leftHand.position = getJointVector3(user, SkeletonJoint.LeftHand);
				//rightHand.position = getJointVector3(user, SkeletonJoint.RightHand);
			}
		}
	}

	Vector3 getJointVector3 (uint user, SkeletonJoint joint)
	{
		SkeletonJointPosition pos = new SkeletonJointPosition ();
		skeletonCapability.GetSkeletonJointPosition (user, joint, ref pos);
		Vector3 v3pos = new Vector3 (-pos.position.X, pos.position.Y, pos.position.Z);
		return v3pos / scale + bias;
	}

	void createDefaultLineRenderer (Transform obj)
	{
		LineRenderer lineRenderer = obj.gameObject.AddComponent (typeof(LineRenderer)) as LineRenderer;
		lineRenderer.material = new Material (Shader.Find ("Particles/Additive"));
		lineRenderer.SetColors (Color.yellow, Color.red);
		lineRenderer.SetWidth (0.2f, 0.2f);
	}

	//--------------------------------

	void gestures_GestureRecognized (ProductionNode node, string strGesture, ref Point3D idPosition, ref Point3D endPosition)
	{
		Debug.Log ("gestures_GestureRecognized(): Here");
		switch (strGesture) {
		case "Wave":
			handsGenerator.StartTracking (ref idPosition);
			break;
		default:
			break;
		}
	}

	void hands_HandDestroy (ProductionNode node, uint id, float fTime)
	{
		Debug.Log ("hands_HandDestroy(): Here");
	}

	void hands_HandUpdate (ProductionNode node, uint id, ref Point3D position, float fTime)
	{
		float x = position.X;
		float y = position.Y;
		float z = position.Z;
		Debug.Log ("hands_HandUpdate(): Position X: " + x + " Y: " + y + " Z: " + z);
		handPoint.x = x;
		handPoint.y = y;
		handPoint.z = z;
		//Vector3 pos = new Vector3(x * scale, y * scale, -z * scale);
		//pos -= bias;
		//if (pos.z > 0) pos.z = 0;
		//hand.position = Vector3.Lerp(hand.position, pos, smoothingSpeed);
	}

	void hands_HandCreate (ProductionNode node, uint id, ref Point3D position, float fTime)
	{
		Debug.Log ("hands_HandCreate(): Here");
		
		float x = position.X;
		float y = position.Y;
		float z = position.Z;
		Debug.Log ("hands_HandUpdate(): Position X: " + x + " Y: " + y + " Z: " + z);
		handPoint.x = x;
		handPoint.y = y;
		handPoint.z = z;
		//Vector3 pos = new Vector3(x * scale, y * scale, -z * scale);
		//bias = pos;
		//bias.z += zBias;
	}
	
}

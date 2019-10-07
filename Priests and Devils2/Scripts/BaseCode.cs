using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.carolsum.PND;
using System;

namespace com.carolsum.PND
{
    public class Director : System.Object
    {
        private static Director _instance;
        public SceneController currentSceneController { get; set; }
        public static Director getInstance()
        {
            if (_instance == null)
            {
                _instance = new Director();
            }
            return _instance;
        }
    }
    
    public interface SceneController
    {
        void LoadResources();
    }
    
    public interface UserAction
    {
        void OnMoveBoat();
        void OnClickedCharacter(Character characterController);
        void Restart();
    }

    public class ClickBehaviour : MonoBehaviour
    {
        private UserAction action;
        private Character controller;

        void Start()
        {
            action = Director.getInstance().currentSceneController as UserAction;
        }

        public void setCharacter(Character character)
        {
            controller = character;
        }

        void OnMouseDown()
        {
            if (gameObject.name == "boat")
            {
                action.OnMoveBoat();
            }
            else
            {
                action.OnClickedCharacter(controller);
            }
        }
    }

    public class Character
    {
        private GameObject character;
        private int characterType; 
        private ClickBehaviour clickBehaviour;

        bool _isOnBoat;
        Coast coastCtrl;

        public Character(string character_type)
        {
            if (character_type == "Priest")
            {
                character = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Priest", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
                characterType = 0;
            }
            else
            {
                character = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Devil", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
                characterType = 1;
            }
            clickBehaviour = character.AddComponent(typeof(ClickBehaviour)) as ClickBehaviour;
            clickBehaviour.setCharacter(this);
        }

        public void setName(string name)
        {
            character.name = name;
        }

        public GameObject getGameObject()
        {
            return character;
        }

        public string getName()
        {
            return character.name;
        }

        public int getType()
        {
            return characterType;
        }

        public void setPosition(Vector3 pos)
        {
            character.transform.position = pos;
        }


        public void getOnBoat(Boat boat)
        {
            coastCtrl = null;
            character.transform.parent = boat.getGameObject().transform;
            _isOnBoat = true;
        }

        public void getOnCoast(Coast coast)
        {
            coastCtrl = coast;
            character.transform.parent = null;
            _isOnBoat = false;
        }

        public bool isOnBoat()
        {
            return _isOnBoat;
        }

        public Coast getCoastController()
        {
            return coastCtrl;
        }

        public void reset()
        {
            coastCtrl = (Director.getInstance().currentSceneController as FirstController).fromCoast;
            getOnCoast(coastCtrl);
            setPosition(coastCtrl.getEmptyPosition());
            coastCtrl.getOnCoast(this);
        }
    }

    public class Coast
    {
        private GameObject coast;
        private Vector3 startPos = new Vector3(9, 1, 0);
        private Vector3 endPos = new Vector3(-9, 1, 0);
        private Vector3[] positions;
        private int startOrEnd; 

        Character[] charactersOnCoast;
  
        public Coast(string _startOrEnd)
        {
            positions = new Vector3[] {new Vector3(6.5F,2.25F,0), new Vector3(7.5F,2.25F,0), new Vector3(8.5F,2.25F,0),
                new Vector3(9.5F,2.25F,0), new Vector3(10.5F,2.25F,0), new Vector3(11.5F,2.25F,0)};
            charactersOnCoast = new Character[6];
            if (_startOrEnd == "start")
            {
                coast = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Stone", typeof(GameObject)), startPos, Quaternion.identity, null) as GameObject;
                coast.name = "start";
                startOrEnd = 1;
            }
            else
            {
                coast = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Stone", typeof(GameObject)), endPos, Quaternion.identity, null) as GameObject;
                coast.name = "end";
                startOrEnd = -1;
            }
        }

        public Vector3 getEmptyPosition()
        {
            Vector3 pos = positions[getEmptyIndex()];
            pos.x *= startOrEnd;
            return pos;
        }

        public int getEmptyIndex()
        {
            for (int i = 0; i < charactersOnCoast.Length; i++)
            {
                if (charactersOnCoast[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }

        public void getOnCoast(Character character)
        {
            int index = getEmptyIndex();
            charactersOnCoast[index] = character;
        }

        public Character getOffCoast(string character_name)
        {
            for (int i = 0; i < charactersOnCoast.Length; i++)
            {
                if (charactersOnCoast[i] != null && charactersOnCoast[i].getName() == character_name)
                {
                    Character temp = charactersOnCoast[i];
                    charactersOnCoast[i] = null;
                    return temp;
                }
            }
            Debug.Log("can not find character on coast: " + character_name);
            return null;
        }

        public int getStartOrEnd()
        {
            return startOrEnd;
        }

        public int[] getCharactersNum()
        {
            int[] count = { 0, 0 };
            for (int i = 0; i < charactersOnCoast.Length; i++)
            {
                if (charactersOnCoast[i] == null) continue;
                if (charactersOnCoast[i].getType() == 0)    
                {
                    count[0]++;
                }
                else
                {
                    count[1]++;
                }
            }
            return count;
        }

        public void reset()
        {
            charactersOnCoast = new Character[6];
        }
    }

    public class Boat
    {
        private GameObject boat;
        private Vector3 fromPosition = new Vector3(5, 1, 0);
        private Vector3 toPosition = new Vector3(-5, 1, 0);
        private Vector3[] from_positions;
        private Vector3[] to_positions;

        int to_or_from; 
        Character[] passenger = new Character[2];

        public Boat()
        {
            to_or_from = 1;
            from_positions = new Vector3[] { new Vector3(4.5F, 1.5F, 0), new Vector3(5.5F, 1.5F, 0) };
            to_positions = new Vector3[] { new Vector3(-5.5F, 1.5F, 0), new Vector3(-4.5F, 1.5F, 0) };

            boat = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Boat", typeof(GameObject)), fromPosition, Quaternion.identity, null) as GameObject;
            boat.name = "boat";
            boat.AddComponent(typeof(ClickBehaviour));
        }

        public int getEmptyIndex()
        {
            for (int i = 0; i < passenger.Length; i++)
            {
                if (passenger[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }

        public Vector3 getEmptyPosition()
        {
            Vector3 pos;
            int emptyIndex = getEmptyIndex();
            if (to_or_from == -1)
            {
                pos = to_positions[emptyIndex];
            }
            else
            {
                pos = from_positions[emptyIndex];
            }
            return pos;
        }

        public bool isEmpty()
        {
            for (int i = 0; i < passenger.Length; i++)
            {
                if (passenger[i] != null)
                {
                    return false;
                }
            }
            return true;
        }

        public void GetOnBoat(Character character)
        {
            int index = getEmptyIndex();
            passenger[index] = character;
        }

        public Character GetOffBoat(string passenger_name)
        {
            for (int i = 0; i < passenger.Length; i++)
            {
                if (passenger[i] != null && passenger[i].getName() == passenger_name)
                {
                    Character charactorCtrl = passenger[i];
                    passenger[i] = null;
                    return charactorCtrl;
                }
            }
            Debug.Log("Can not find passenger in boat: " + passenger_name);
            return null;
        }

        public GameObject getGameObject()
        {
            return boat;
        }

        public int getToOrFrom()
        {
            return to_or_from;
        }

        public int[] getCharacterNum()
        {
            int[] count = { 0, 0 };
            for (int i = 0; i < passenger.Length; i++)
            {
                if (passenger[i] == null)
                {
                    continue;
                }
                if (passenger[i].getType() == 0)
                {
                    count[0]++;
                }
                else
                {
                    count[1]++;
                }
            }
            return count;
        }

        public void reset()
        {
            if (to_or_from == -1)
            {
                var actionManager = ((FirstController)Director.getInstance().currentSceneController).actionManager;
                actionManager.moveBoat(boat, to_or_from);
                to_or_from = 1;
            }
            passenger = new Character[2];
        }

        public void changeToOrFrom()
        {
            if (to_or_from == -1) to_or_from = 1;
            else to_or_from = -1;
        }
    }
    
    public class SSAction : ScriptableObject
    {
        public bool enabled = true;
        public bool destory = false;

        public GameObject gameObject { get; set; }
        public Transform transform { get; set; }
        public ISSActionCallBack callback { get; set; }

        protected SSAction() { }

        public virtual void Start() { throw new System.NotImplementedException(); }

        public virtual void Update() { throw new System.NotImplementedException(); }
    }
    
    public enum SSActionEventType : int { Started, Completed };
    
    public interface ISSActionCallBack
    {
        void SSActionEvent(SSAction sourse, SSActionEventType events = SSActionEventType.Completed);
    }
    
    public class SSActionManager : MonoBehaviour
    {
        private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
        private List<SSAction> waitingAdd = new List<SSAction>();
        private List<int> waitingDelete = new List<int>();

        protected void Update()
        {
            foreach (SSAction ac in waitingAdd) actions[ac.GetInstanceID()] = ac;
            waitingAdd.Clear();
            foreach (KeyValuePair<int, SSAction> kv in actions)
            {
                SSAction ac = kv.Value;
                if (ac.destory)
                {
                    waitingDelete.Add(ac.GetInstanceID());
                }
                else if (ac.enabled)
                {
                    ac.Update();
                }
            }

            foreach (int key in waitingDelete)
            {
                SSAction ac = actions[key];
                actions.Remove(key);
                DestroyObject(ac);
            }
            waitingDelete.Clear();
        }

        protected void Start() { }

        public void RunAction(GameObject gameObject, SSAction action, ISSActionCallBack manager)
        {
            action.gameObject = gameObject;
            action.callback = manager;
            action.transform = gameObject.transform;
            waitingAdd.Add(action);
            action.Start();
        }
    }
    
    public class CCActionManager : SSActionManager, ISSActionCallBack
    {
        public FirstController sceneController;

        public void moveBoat(GameObject boat, int to_or_from)
        {
            if (to_or_from == -1)
            {
                Debug.Log("船应该向右移");
                CCBoatMove boatMoveToRight = CCBoatMove.GetSSAction(new Vector3(5, 1, 0));
                this.RunAction(boat, boatMoveToRight, this);
            }
            else
            {
                Debug.Log("船应该向左移");
                CCBoatMove boatMoveToLeft = CCBoatMove.GetSSAction(new Vector3(-5, 1, 0));
                this.RunAction(boat, boatMoveToLeft, this);
            }
        }

        public void moveCharacter(GameObject obj, Vector3 dest)
        {
            CCCharacterMove characterMove = CCCharacterMove.GetSSAction(dest);
            this.RunAction(obj, characterMove, this);
        }

        protected new void Start()
        {
            sceneController = (FirstController)Director.getInstance().currentSceneController;
            sceneController.actionManager = this;
        }

        protected new void Update()
        {
            base.Update();
        }

        public void SSActionEvent(SSAction sourse, SSActionEventType events = SSActionEventType.Completed)
        {
            Debug.Log("Completed!");
        }
    }

    public class CCBoatMove : SSAction
    {
        public Vector3 target;
        public float speed = 20;

        public static CCBoatMove GetSSAction(Vector3 target)
        {
            CCBoatMove action = ScriptableObject.CreateInstance<CCBoatMove>();
            action.target = target;
            return action;
        }

        public override void Start()
        {
            Debug.Log("开始移动");
            this.transform.position = Vector3.MoveTowards(this.transform.position, this.target, this.speed * Time.deltaTime);
        }

        public override void Update()
        {
            if (this.transform.position == target)
            {
                Debug.Log("移动结束");
                this.destory = true;
                this.callback.SSActionEvent(this);
            }
            else
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, this.target, this.speed * Time.deltaTime);
            }
        }
    }
    
    public class CCCharacterMove : SSAction
    {
        private Vector3 target;
        private Vector3 mid;
        int status = 0; 
        public float speed = 20;

        public static CCCharacterMove GetSSAction(Vector3 target)
        {
            CCCharacterMove action = ScriptableObject.CreateInstance<CCCharacterMove>();
            action.target = target;
            action.mid = target;
            return action;
        }

        public override void Start()
        {
            Debug.Log("开始移动");
            if (target.y == transform.position.y)
            {
                status = 2;
            }
            else if (target.y < transform.position.y)
            {
                mid.y = transform.position.y;
            }
            else
            {
                mid.x = transform.position.x;
            }

            status = 1;
        }

        public override void Update()
        {
            if (status == 1)
            {
                transform.position = Vector3.MoveTowards(transform.position, mid, speed * Time.deltaTime);
                if (transform.position == mid)
                {
                    status = 2;
                }
            }
            else if (status == 2)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                if (transform.position == target)
                {
                    status = 0;
                    Debug.Log("移动结束");
                    this.destory = true;
                    this.callback.SSActionEvent(this);
                }
            }
        }
    }

}


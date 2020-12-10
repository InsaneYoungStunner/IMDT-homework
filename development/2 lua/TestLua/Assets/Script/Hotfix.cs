using UnityEngine;
using UnityEngine.UI;
using XLua;

namespace XLuaTest
{
    [Hotfix]
    [LuaCallCSharp]
    public class Hotfix : MonoBehaviour
    {
        LuaEnv luaenv = new LuaEnv();

        // Use this for initialization
        public int hotfixValue = 1;
        void Start()
        {
        
        }

        public Text showText;
        void Update()
        {
           
        }



        public void replacedCode()
        {
            print("replacedCode");
        }

        public void displayUpdate()
        {
            replacedCode();
            print("displayUpdate");
            if (hotfixValue <= 0.2)
            {
                showText.text = "替换成功。hotfixValue已被更新，replacedCode的代码已被替换为更新hotfixValue的代码";
            }
            else
            {
                showText.text = "hotfixValue为1，没有变化。请单击热更新按钮替换C#方法来更改replacedCode";
            }
        }


        public void BeginHotFix()
        {

            print("BeginHotFix");
            
            luaenv.DoString(@"
                xlua.hotfix(CS.XLuaTest.Hotfix, 'replacedCode', function(self) 
	                    print('hotfixValue Before:',hotfixValue)
	                    self.hotfixValue=0.1
	                    print('hotfixValue After:',hotfixValue)
                end)
            ");
        }

       
        
    }
}

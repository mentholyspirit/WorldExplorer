using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;

namespace Decentraland.Task1
{
    public class WorldExplorer : IDisposable
    {
        private static class Logger
        {
            public static void log(string message)
            {
                Console.WriteLine(message);
            }
        }

        private class EngineModule
        {
            public object sendMessage(string message)
            {
                return SendMessageInternal(message).ToPromise();
            }

            private async Task SendMessageInternal(string message)
            {
                await Task.Delay(500);
                Console.WriteLine("EngineModule: sendMessage: " + message);
                await Task.Delay(500);
            }
        }

        private class ModuleLoader
        {
            private V8ScriptEngine m_Engine;
            public ModuleLoader(V8ScriptEngine engine)
            {
                m_Engine = engine;
            }
            public object Require(string moduleName)
            {
                if (moduleName.Equals("~engine"))
                {
                    m_Engine.AddHostType(typeof(EngineModule));
                    return m_Engine.Evaluate("new EngineModule()");
                }
                throw new EntryPointNotFoundException("Unknown module");
            }
        }

        private V8ScriptEngine m_Engine;
        private ModuleLoader m_ModuleLoader;

        public WorldExplorer()
        {
            m_Engine = new V8ScriptEngine();
            m_ModuleLoader = new ModuleLoader(m_Engine);
            m_Engine.AccessContext = typeof(WorldExplorer);
            m_Engine.AddHostObject("moduleLoader", m_ModuleLoader);
            m_Engine.AddHostType("console", typeof(Logger));
            m_Engine.Execute(@"
                const require = moduleLoader.Require
                let module = {
                    exports : {
                        onStart : async function() { }
                    }
                }");
        }

        public void Dispose()
        {
            m_Engine.Dispose();
        }

        public void LoadScene(string scene)
        {
            m_Engine.Execute(scene);
        }

        public async Task Start(int framesToRun)
        {
            await m_Engine.Evaluate(@"module.exports.onStart()").ToTask();
            for (int frame = 0; frame < framesToRun; ++frame)
            {
                await m_Engine.Evaluate(@"module.exports.onUpdate(" + (frame + 1)+ ")").ToTask();
            }
        }
    }
}
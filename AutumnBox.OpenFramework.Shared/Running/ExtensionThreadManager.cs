﻿using AutumnBox.OpenFramework.Open;
using AutumnBox.OpenFramework.Wrapper;
using System;
using System.Collections.Generic;

namespace AutumnBox.OpenFramework.Running
{
#if SDK
    internal 
#else
    public
#endif

    class ExtensionThreadManager : IExtensionThreadManager
    {
        public static ExtensionThreadManager Instance { get; }
        private readonly List<IExtensionThread> readys = new List<IExtensionThread>();
        private readonly List<IExtensionThread> runnings = new List<IExtensionThread>();

        public IExtensionThread Allocate(IExtensionWrapper wrapper, Type typeOfExtension)
        {
            var thread = new ExtensionThread(this, typeOfExtension, wrapper)
            {
                Id = AlllocatePID()
            };
            readys.Add(thread);
            thread.Started += (s, e) =>
            {
                readys.Remove(thread);
                runnings.Add(thread);
            };
            thread.Finished += (s, e) =>
            {
                runnings.Remove(thread);
            };
            return thread;
        }

        private readonly Random ran = new Random();

        static ExtensionThreadManager()
        {
            Instance = new ExtensionThreadManager();
        }
        private ExtensionThreadManager() { }
        private int AlllocatePID()
        {
            int nextPid;
            do
            {
                nextPid = ran.Next();
            } while (FindThreadById(nextPid) != null);
            return nextPid;
        }

        public IExtensionThread FindThreadById(int id)
        {
            return runnings.Find((thr) =>
            {
                return thr.Id == id;
            });
        }

        public IEnumerable<IExtensionThread> GetRunning()
        {
            return runnings;
        }
    }
}

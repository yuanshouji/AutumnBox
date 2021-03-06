﻿/*************************************************
** auth： zsh2401@163.com
** date:  2018/10/10 19:13:50 (UTC +8:00)
** desc： ...
*************************************************/
using System;
using System.Collections.Generic;
using AutumnBox.OpenFramework.Content;
using AutumnBox.OpenFramework.Exceptions;
using AutumnBox.OpenFramework.Running;
using AutumnBox.OpenFramework.Wrapper;

namespace AutumnBox.OpenFramework.Extension
{
    /// <summary>
    /// 基础的IClassExtension的基础实现
    /// </summary>
    [ExtName("无名拓展", "en-us:Unknown extension")]
    [ExtAuth("佚名", "en-us:Anonymous")]
    [ExtDesc(null)]
    [ExtVersion()]
    [ExtRequiredDeviceStates(AutumnBoxExtension.NoMatter)]
    [ExtMinApi(8)]
    [ExtTargetApi()]
    [ExtRunAsAdmin(false)]
    [ExtRequireRoot(false)]
    [ExtOfficial(false)]
    [ExtRegions(null)]
    [ExtPriority(ExtPriority.NORMAL)]
    [ExtDeveloperMode(false)]
    //[ExtAppProperty("com.miui.fm")]
    //[ExtMinAndroidVersion(7,0,0)]
    public abstract class ClassExtensionBase : Context, IClassExtension
    {
        /// <summary>
        /// 切面阻止了...
        /// </summary>
        public class AspectPreventedException :Exception { }
        /// <summary>
        /// 构造
        /// </summary>
        public ClassExtensionBase()
        {
            var scanner = new ClassExtensionScanner(GetType());
            scanner.Scan(ClassExtensionScanner.ScanOption.BeforeCreatingAspect);
            var bcAspects = scanner.BeforeCreatingAspects;
            bool canContinue = true;
            BeforeCreatingAspectArgs bcaArgs = new BeforeCreatingAspectArgs(this, GetType());
            foreach (var aspect in bcAspects)
            {
                aspect.BeforeCreating(bcaArgs, ref canContinue);
                if (!canContinue)
                {
                    throw new AspectPreventedException();
                }
            }
        }

        /// <summary>
        /// 当拓展被创建后调用
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnCreate(ExtensionArgs args)
        {
        }

        /// <summary>
        /// 主函数发生异常时调用
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnExcetpion(Exception e) { }

        /// <summary>
        /// 无论如何,在模块即将被析构时,都将调用此函数
        /// </summary>
        protected virtual void OnDestory(object args) { }

        /// <summary>
        /// 当模块被要求终止时调用,如果做不到,请返回false或抛出异常
        /// </summary>
        /// <returns></returns>
        protected virtual bool OnStopCommand(object args)
        {
            return true;
        }

        /// <summary>
        /// 接收信号
        /// </summary>
        /// <param name="signalName"></param>
        /// <param name="value"></param>
        public virtual void ReceiveSignal(string signalName, object value = null)
        {
            switch (signalName)
            {
                case Signals.COMMAND_DESTORY:
                    OnDestory(value);
                    break;
                case Signals.COMMAND_STOP:
                    if (!OnStopCommand(value))
                    {
                        throw new ExtensionCantBeStoppedException("Cant stop!", null);
                    }
                    break;
                case Signals.ON_CREATED:
                    OnCreate(value as ExtensionArgs);
                    break;
                case Signals.ON_EXCEPTION:
                    OnExcetpion(value as Exception);
                    break;
                default:
                    OnReceiveUnknownSignal(signalName, value);
                    break;
            }
        }

        /// <summary>
        /// 接收到无法被处理的信号时调用
        /// </summary>
        /// <param name="signalName"></param>
        /// <param name="value"></param>
        protected virtual void OnReceiveUnknownSignal(string signalName, object value)
        {

        }

        /// <summary>
        /// 主要方法
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public abstract int Main(Dictionary<string, object> data);
    }
}

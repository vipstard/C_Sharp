﻿using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Threaddd
{
    //BackgroundWorker 클래스는 쓰레드풀에서 작업 쓰레드(Worker Thread)를 할당 받아 작업을
    //실행하는 Wrapper 클래스이다.BackgroundWorker는 이벤트를 기반으로 비동기 처리를
    //진행하는 패턴(Event-based Asynchronous Pattern)을 구현한 클래스이다.BackgroundWorker로부터
    //생성된 객체는 DoWork 이벤트 핸들러를 통해 실제 작업할 내용을 지정하고, RunWorkerAsync()
    //메서드를 호출하여 작업을 시작한다.
    public class BackgroundWorker_
    {
        static void Main(String[] args)
        {
            new BackgroundWorker_().Execute();
            Console.ReadLine();

        }

        private BackgroundWorker worker;

        public void Execute()
        {
            /// 쓰레드풀에서 작업 쓰레드 시작
            worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // 긴 처리 가정
            Console.WriteLine("Long running task");
        }



    }
}

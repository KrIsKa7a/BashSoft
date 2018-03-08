using BashSoft.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.IO.Commands
{
    public abstract class Command
    {
        private string input;
        private string[] data;
        private Tester judge;
        private StudentRepository studentRepository;
        private IOManager inputOutputManager;

        public Command(string input, string[] data, Tester judge, StudentRepository studentRepository, IOManager iOManager)
        {
            this.Input = input;
            this.Data = data;
            this.judge = judge;
            this.studentRepository = studentRepository;
            this.inputOutputManager = iOManager;
        }

        protected string Input
        {
            get { return this.input; }
            private set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new InvalidStringException();
                }

                this.input = value;
            }
        }

        protected string[] Data
        {
            get { return this.data; }
            private set
            {
                if (value == null || value.Length == 0)
                {
                    throw new NullReferenceException();
                }

                this.data = value;
            }
        }

        protected Tester Judge
        {
            get { return this.judge; }
        }

        protected StudentRepository StudentRepository
        {
            get { return this.studentRepository; }
        }

        protected IOManager InputOutputManager
        {
            get { return this.inputOutputManager; }
        }

        public abstract void Execute();
    }
}

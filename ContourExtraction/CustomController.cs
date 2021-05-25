using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContourExtraction
{
    public class CustomController
    {

        private readonly Func<string> _inputProvider;
        private readonly Action<string> _outputProvider;
        private readonly Actions _actions;
        private string _outfilepath = "";


        /// <summary>
        /// Custom controller constructor using Dependecy Injection for handling console input/output and given yuv model.  
        /// </summary>
        public CustomController(Func<string> inputProvider, Action<string> outputProvider, Actions actions)
        {
            _inputProvider = inputProvider;
            _outputProvider = outputProvider;
            _actions = actions;
        }



        /// <summary>
        /// Reads from a .yuv file and gets all the essential information about it.
        /// </summary>
        public CustomController Build()
        {

            _actions.GetInformation();

            if (!_actions.ReadFile().IsCompletedSuccessfully)
            {
                var ex = _actions.ReadFile().Exception;
                Console.WriteLine($"\n {ex.Message} \n");
                Environment.Exit(0);
            }
    
            return this;
        }



        /// <summary>
        /// Executes the required task, if build was successfull.
        /// </summary>
        public CustomController Run()
        {
            
            return this;
        }



        /// <summary>
        /// Writes the output image to a new .yuv file.
        /// </summary>
        public CustomController Out()
        {

            _outfilepath = _actions.CreateFilePath();
            _actions.WriteToFile();
            _outputProvider($"\n\n  Your file is ready to use at the following path:\n  {_outfilepath}");

            return this;
        }


    }
}

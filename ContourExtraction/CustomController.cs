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
        private readonly Contour _contour;
        private string _outfilepath = "";


        /// <summary>
        /// Custom controller constructor using Dependecy Injection for handling console input/output and given yuv model.  
        /// </summary>
        public CustomController(Func<string> inputProvider, Action<string> outputProvider, Actions actions, Contour contour)
        {
            _inputProvider = inputProvider;
            _outputProvider = outputProvider;
            _actions = actions;
            _contour = contour;
        }



        /// <summary>
        /// Handles the required actions needed for reading a .yuv file.
        /// </summary>
        public CustomController Build()
        {

            _actions.GetInformation();

            if (!_actions.ReadFile().IsCompletedSuccessfully)
            {
                var ex = _actions.ReadFile().Exception;
                Console.WriteLine($"\n {ex.Message} \n");
                Environment.Exit(-1);
            }
    
            return this;
        }



        /// <summary>
        /// Executes the contour tracing task, if build was successfull.
        /// </summary>
        public CustomController Run()
        {
            if (!_contour.ContourTracing().IsCompletedSuccessfully)
            {
                var ex = _contour.ContourTracing().Exception;
                Console.WriteLine($"\n {ex} \n");
                Environment.Exit(-1);
            }

            return this;
        }



        /// <summary>
        /// Writes the generated image to a new .yuv file.
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

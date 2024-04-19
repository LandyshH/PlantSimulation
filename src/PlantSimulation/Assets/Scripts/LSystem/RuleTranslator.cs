using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.LSystem
{
    public class RuleTranslator
    {
        private string _sentence;
        private int _iterations;

        public RuleTranslator(string sentence, int iterations) 
        {
            _sentence = sentence;
            _iterations = iterations;
        }

        public void Translate()
        {
            foreach (var c in _sentence)
            {
                switch (c)
                {

                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    [System.Serializable]
    public class Wave {
        public Wave() {
            if(sections == null)
                sections = new List<WaveSection>();
        }

        public List<WaveSection> sections;
    }
}


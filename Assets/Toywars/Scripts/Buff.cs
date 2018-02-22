using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toywars {
    public interface Buff {
        void apply(Component target);
        void tick();
        bool finished { get; }
    }
}

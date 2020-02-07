using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDEN {
    public static class MathHelper {

        public static double DegreeToRadian(float angle) {
            // Converts degrees to radians
            return Math.PI * angle / 180f;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dino
{
    class TemplateGPX
    {
        public static string tag = @"
        new L.GPX('[FILE_NAME]', {
            async: true,
            marker_options: {				
                startIconUrl: '[ICON_START]',
                endIconUrl: '[ICON_END]',
                shadowUrl: ''
            },
            polyline_options: {
                color: '#[COLOR]',
                weight: [WEIGHT]
            }
        }).addTo(map);";
    }
}

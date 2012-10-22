using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Television.Models.Enums;
using System.Collections.ObjectModel;

namespace Television.Interfaces
{
    public interface ITelevision
    {
        /// <summary>
        /// Current State of Television ie ON/OFF
        /// </summary>
        TelevisionStateEnum State { get; set; }
               
        /// <summary>
        /// Current channel
        /// </summary>
        int Channel { get; set; }
    }
}

using System;
using System.Net;
using System.Linq;
using System.Diagnostics;
using Television.Models.Enums;
using Television.Interfaces;
using System.Collections.ObjectModel;

namespace Television.Models
{    
    public class Television : ITelevision
    {                                      
        private TelevisionStateEnum m_State = TelevisionStateEnum.OFF;

        /// <summary>
        /// Sets and gets the State property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public TelevisionStateEnum State
        {
            get
            {
                return m_State;
            }

            set
            {
                if (m_State == value)
                {
                    return;
                }

                m_State = value;
            }
        }       

        private int m_Channel = MIN_CHANNEL;

        /// <summary>
        /// Sets and gets the Channel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int Channel
        {
            get
            {
                return m_Channel;
            }

            set
            {
                if (m_Channel == value)
                {
                    return;
                }

                // Verify we are within hardware limits of TV
                if (value < 1) m_Channel = m_MaxChannel;
                else if (value > m_MaxChannel) m_Channel = MIN_CHANNEL;
                else m_Channel = value;
            }
        }

        internal int m_MaxChannel;
        private const int MIN_CHANNEL = 1;

        public Television(int maxChannel)
        {
            m_MaxChannel = maxChannel;
        }
    }   
}

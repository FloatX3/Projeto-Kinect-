﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSKinectSkeletonApplication1
{
    using System;

    /// <summary>
    /// Event arguments for SpeechRecognizer.
    /// </summary>
    public class SpeechRecognizerEventArgs : EventArgs
    {
        /// <summary>
        /// Speech phrase (text) recognized.
        /// </summary>
        public string Phrase { get; set; }

        /// <summary>
        /// Semantic value associated with recognized speech phrase.
        /// </summary>
        public string SemanticValue { get; set; }

        /// <summary>
        /// Best guess at source angle from which speech command originated.
        /// </summary>
        public double? SourceAngle { get; set; }
    }
}

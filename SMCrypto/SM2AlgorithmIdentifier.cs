using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Text;

namespace HCenter.Encryption.SMCrypto
{
    internal class SM2AlgorithmIdentifier : AlgorithmIdentifier
    {

        private readonly bool parametersDefined;

        public SM2AlgorithmIdentifier(DerObjectIdentifier objectID) : base(objectID)
        {

        }


        public SM2AlgorithmIdentifier(DerObjectIdentifier objectID, Asn1Encodable parameters) : base(objectID, parameters)
        {
            this.parametersDefined = true;
        }

        /** 
             * Produce an object suitable for an Asn1OutputStream. 
             *          *      AlgorithmIdentifier ::= Sequence { 
             *                            algorithm OBJECT IDENTIFIER, 
             *                            parameters ANY DEFINED BY algorithm OPTIONAL } 
             *  
        */
        public override Asn1Object ToAsn1Object()
        {
            DerObjectIdentifier sm2Identifier = new DerObjectIdentifier("1.2.156.10197.1.301");
            Asn1EncodableVector v = new Asn1EncodableVector(base.ObjectID, sm2Identifier);
            return new DerSequence(v);
        }

    }
}

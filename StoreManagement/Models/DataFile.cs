using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreManagement.Models
{
    public class DataFile
    {
        private String _FileName;
        private String _ContentType;
        private Byte[] _FileData;

        public DataFile()
        {
            _FileName = null;
            _ContentType = null;
            _FileData = null;
        }



        public String FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }

        public String ContentType
        {
            get { return _ContentType; }
            set { _ContentType = value; }
        }

        public Byte[] FileData
        {
            get { return _FileData; }
            set { _FileData = value; }
        }
    }
}

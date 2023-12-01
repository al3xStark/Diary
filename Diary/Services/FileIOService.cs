using Diary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Services
{
    class FileIOService
    {
        private readonly string PATH;

        public FileIOService(string path)
        {
            PATH = path;
        }

        public BindingList<DayModel> LoadData()
        {
            var fileExists = File.Exists(PATH);
            if (!fileExists)
            {
                File.CreateText(PATH).Dispose();
                return new BindingList<DayModel>();
            }
            return LoadFromFile(PATH);
        }

        public BindingList<DayModel> LoadFromFile(string path)
        {
            using (var reader = File.OpenText(path))
            {
                var fileText = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<BindingList<DayModel>>(fileText);
            }
        }

        public void SaveData(object diaryDataList, string path = "")
        {
            if (path == "") path = PATH;
            using (StreamWriter writer = File.CreateText(path))
            {
                string output = JsonConvert.SerializeObject(diaryDataList);
                writer.Write(output);
            }
        }

        public void SaveDoc(string savedString, string path)
        {
            using (StreamWriter writer = File.CreateText(path))
            {
                writer.Write(savedString);
            }
        }

    }
}

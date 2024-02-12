using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodotToolkit.Services
{
    public class FolderPicker : IFolderPicker
    {
        public string PickFolder()
        {
            try
            {
                string selectedFolderPath = string.Empty;

                using (var folderBrowserDialog = new FolderBrowserDialog())
                {
                    folderBrowserDialog.Description = "Choose the folder path";

                    DialogResult result = folderBrowserDialog.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        // 獲取用戶選擇的資料夾路徑
                        selectedFolderPath = folderBrowserDialog.SelectedPath;

                    }
                }
                return selectedFolderPath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

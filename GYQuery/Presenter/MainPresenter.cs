using GYQuery.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYQuery.Presenter
{
    public class MainPresenter
    {
        private readonly IMainForm _view;
        private readonly IFileManager _meneger;
        private readonly IMessageService _messegeService;

        private string _currentKeyFilePath;
        private string _currentSavePath;
        private int _currentFindNum;

        public MainPresenter(IMainForm view, IFileManager manager, IMessageService service)
        {
            _view = view;
            _meneger = manager;
            _messegeService = service;

            _view.Save += _view_Save;
        }

        private async void _view_Save(object sender, EventArgs e)
        {
            try
            {
                string exelPath = _view.SelectKeyFile;

                bool isExist = _meneger.IsExist(exelPath);

                if (!isExist)
                {
                    _messegeService.ShowExclamation("Выбранный фаил не существует.");
                    return;
                }

                _currentKeyFilePath = exelPath;

                string xmlsPath = _view.SelectSaveFolderPath;

                bool isFolderExist = _meneger.IsFolderExist(xmlsPath);

                if (!isFolderExist)
                {
                    _messegeService.ShowExclamation("Выбранная папка не существует.");
                    return;
                }

                _currentSavePath = xmlsPath;

                if(!int.TryParse(_view.FindNum, NumberStyles.Integer, CultureInfo.InvariantCulture, out _currentFindNum))
                {
                    _messegeService.ShowExclamation("Введите целое число.");
                    return;
                }
            }
            catch (Exception ex)
            {
                _messegeService.ShowError(ex.Message);
            }

            try
            {
                await _meneger.MakeFiles(_currentKeyFilePath, _currentSavePath, _currentFindNum, null);
            }
            catch (Exception ex)
            {
                _messegeService.ShowError(ex.Message);
            }
            _messegeService.ShowMessage("End");
        }
    }
}

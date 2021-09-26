using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Quanda.Client.Components;
using Quanda.Client.Shared.RightMenu;
using Quanda.Shared.DTOs.Requests;

namespace Quanda.Client.Pages.Recover
{
    public partial class RecoverPassword
    {
        /// <summary>
        ///     Model formularza
        /// </summary>
        private RecoverDTO _recoverDto = new();

        /// <summary>
        ///     Zmienna mowiąca o tym, czy formularz jest w trakcie przetwarzania
        /// </summary>
        private bool _isPending;

        /// <summary>
        ///     Referencja do komponentu formularza
        /// </summary>
        private GenericFormComponent<RecoverDTO> _form;

        /// <summary>
        ///     Metoda wysyłająca oraz obsługująca żądanie odzyskania hasła - wywoływana przy potwierdzeniu prawidłowo
        ///     wypełnionego formularza
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        private async Task RecoverPasswordAsync()
        {
            _isPending = true;
            _recoverDto.CaptchaResponseToken = await _form.CaptchaComponent.GetCaptchaResponseAsync();

            var isRecovered = await _usersRepository.RecoverPasswordAsync(_recoverDto);
            if (!isRecovered)
            {
                _toastService.ShowError("Invalid reCaptcha.");
                await _form.CaptchaComponent.ResetCaptchaAsync();
            }
            else
            {
                _toastService.ShowSuccess("Password recovery email has been send.");
                _recoverDto = new RecoverDTO();
            }

            _isPending = false;
        }
    }
}

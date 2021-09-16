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
    public partial class RecoverConfirmationEmail
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
        ///     Metoda wysyłająca oraz obsługująca żądanie - wywoływana po zatwierdzeniu poprawnie wypełnionego formularza
        /// </summary>
        /// <returns></returns>
        public async Task RecoverConfirmationEmailAsync()
        {
            _isPending = true;
            _recoverDto.CaptchaResponseToken = await _form.CaptchaComponent.GetCaptchaResponseAsync();

            var isRecovered = await _usersRepository.RecoverConfirmationEmailAsync(_recoverDto);
            if (!isRecovered)
            {
                _toastService.ShowError("Invalid reCaptcha.");
            }
            else
            {
                _toastService.ShowSuccess("Confirmation email has been re-send.");
                _recoverDto = new RecoverDTO();
            }

            await _form.CaptchaComponent.ResetCaptchaAsync();
            _isPending = false;
        }
    }
}

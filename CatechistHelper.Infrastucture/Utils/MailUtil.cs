using System.Net.Mail;
using System.Net;
using CatechistHelper.Domain.Common;

namespace CatechistHelper.Infrastructure.Utils
{
    public static class MailUtil
    {
        public static string SendEmail(string to, string subject, string body, string? attachFile = null)
        {
            try
            {
                MailMessage msg = new(AppConfig.MailSetting.EmailSender, to, subject, body)
                {
                    IsBodyHtml = true
                };

                using var client = new SmtpClient(AppConfig.MailSetting.HostEmail, AppConfig.MailSetting.PortEmail);
                client.EnableSsl = true;
                if (!string.IsNullOrEmpty(attachFile))
                {
                    Attachment attachment = new(attachFile);
                    msg.Attachments.Add(attachment);
                }
                NetworkCredential credential = new(AppConfig.MailSetting.EmailSender, AppConfig.MailSetting.PasswordSender);
                client.UseDefaultCredentials = false;
                client.Credentials = credential;
                client.Send(msg);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return "";
        }
    }

    public static class ContentMailUtil
    {
        public readonly static string Title_ThankingForRegistration = "[Catechist Helper] Xác nhận nộp hồ sơ thành công - Giáo lý viên";
        public readonly static string Title_AnnounceRejectRegistration = "[Catechist Helper] Thông báo về kết quả hồ sơ ứng tuyển - Giáo lý viên";
        public readonly static string Title_AnnounceInterviewSchedule = "[Catechist Helper] Thư mời phỏng vấn - Giáo lý viên";
        public readonly static string Title_AnnounceUpdateInterviewSchedule = "[Catechist Helper] Cập nhật lịch phỏng vấn - Giáo lý viên";
        public readonly static string Title_AnnounceApproveInterview = "[Catechist Helper] Thông báo kết quả phỏng vấn - Chúc mừng bạn đã trúng tuyển";
        public readonly static string Title_AnnounceRejectInterview = "[Catechist Helper] Thông báo kết quả phỏng vấn - Giáo lý viên";
        public readonly static string Title_AccountInformation = "[Catechist Helper] Thông báo thông tin tài khoản đăng nhập";
        public readonly static string INTERVIEW_ADDRESS = "Giáo xứ Catechist Helper, 130 Lê Lợi, Phường Bến Nghé, Quận 1, TP. Hồ Chí Minh";

        public static string ThankingForRegistration(string fullname)
        {
            return @"<!doctype html>
<html>
  <head>
    <title></title>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1"" />
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"" />
    <style type=""text/css"">
      body {
        font-family: sans-serif;
      }

      body,
      table,
      td,
      a {
        -webkit-text-size-adjust: 100%;
        -ms-text-size-adjust: 100%;
      }

      table,
      td {
        mso-table-lspace: 0pt;
        mso-table-rspace: 0pt;
      }

      img {
        -ms-interpolation-mode: bicubic;
      }

      /* RESET STYLES */
      img {
        border: 0;
        height: auto;
        line-height: 100%;
        outline: none;
        text-decoration: none;
      }

      table {
        border-collapse: collapse !important;
      }

      body {
        height: 100% !important;
        margin: 0 !important;
        padding: 0 !important;
        width: 100% !important;
      }

      /* iOS BLUE LINKS */
      a[x-apple-data-detectors] {
        color: inherit !important;
        text-decoration: none !important;
        font-size: inherit !important;
        font-family: inherit !important;
        font-weight: inherit !important;
        line-height: inherit !important;
      }

      /* MOBILE STYLES */
      @media screen and (max-width: 600px) {
        h1 {
          font-size: 32px !important;
          line-height: 32px !important;
        }
      }

      /* ANDROID CENTER FIX */
      div[style*=""margin: 16px 0;""] {
        margin: 0 !important;
      }
    </style>
  </head>

  <body
    style=""
      background-color: #f4f4f4;
      margin: 0 !important;
      padding: 0 !important;
    ""
  >
    <!-- HIDDEN PREHEADER TEXT -->
    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
      <tbody>
        <tr>
          <td bgcolor=""#422A14"" align=""center"">
            <table
              border=""0""
              cellpadding=""0""
              cellspacing=""0""
              width=""100%""
              style=""max-width: 600px""
            >
              <tbody>
                <tr>
                  <td
                    align=""center""
                    valign=""top""
                    style=""padding: 30px 10px""
                  ></td>
                </tr>
              </tbody>
            </table>
          </td>
        </tr>
        <tr>
          <td bgcolor=""#422A14"" align=""center"" style=""padding: 0px 10px"">
            <table
              border=""0""
              cellpadding=""0""
              cellspacing=""0""
              width=""100%""
              style=""max-width: 800px; height: auto""
            >
              <tbody>
                <tr>
                  <td
                    bgcolor=""#ffffff""
                    align=""center""
                    valign=""top""
                    style=""
                      padding: 30px 50px;
                      border-radius: 4px 4px 0px 0px;
                      color: #000;
                      font-weight: 400;
                      text-align: left;
                    ""
                  >
                    <h1
                      style=""
                        font-weight: bolder;
                        font-size: 24px;
                        color: #422a14;
                        margin: 0px;
                        margin-bottom: 10px;
                        text-align: left;
                      ""
                    >
                      Catechist Helper
                    </h1>
                    <h1
                      style=""
                        font-size: 18px;
                        font-weight: 400;
                        margin: 0px;
                        margin-bottom: 10px;
                        color: #422a14;
                        text-align: left;
                        font-weight: 600;
                      ""
                    >
                      Thân gửi " + fullname + @"
                    </h1>
                    <p
                      style=""
                        margin: 0px;
                        text-align: left;
                        line-height: 20px;
                      ""
                    >
                      Chúng tôi rất vui mừng thông báo rằng chúng tôi đã nhận
                      được hồ sơ đăng ký của bạn cho vị trí Giáo lý viên tại
                      Giáo xứ của chúng tôi. Hồ sơ của bạn sẽ được xem xét kỹ
                      lưỡng trong các vòng tiếp theo của quy trình tuyển dụng.
                    </p>
                    <p>
                      <strong>Quy trình tuyển dụng</strong> sẽ bao gồm các vòng sau:

                    <ol>
                      <li style=""margin-bottom: 5px;""><strong>Xét duyệt hồ sơ:</strong> Chúng tôi sẽ xem xét hồ sơ của bạn và so sánh với các yêu cầu của vị trí.
                      </li>
                      <li style=""margin-bottom: 5px;""><strong>Phỏng vấn:</strong> Nếu hồ sơ của bạn phù hợp, chúng tôi sẽ sắp xếp một buổi phỏng vấn.
                      </li>
                      <li style=""margin-bottom: 0px;""><strong>Thông báo kết quả cuối cùng:</strong> Kết quả sẽ được gửi sau khi quy trình phỏng vấn hoàn tất.
                      </li>
                    </ol>                   
                    </p>
                    <p style=""margin:0; padding: 0; margin-bottom: 15px;
                        line-height: 20px;"">
                      Hiện chúng tôi đang trong quá trình xét duyệt hồ sơ và sẽ gửi 
                      đến bạn kết quả nhanh nhất. Xin chân thành cảm ơn bạn đã quan tâm và dành thời gian để nộp đơn ứng tuyển.
                    </p>
                    <p style=""margin:0; padding: 0;"">
                      Trân trọng,</p>
                      <p style=""margin:0; padding: 0; margin-top: 2px;"">
                        Catechist Helper</p>
                  </td>
                </tr>
              </tbody>
            </table>
          </td>
        </tr>
        <tr>
          <td bgcolor=""#422A14"" align=""center"">
            <table
              border=""0""
              cellpadding=""0""
              cellspacing=""0""
              width=""100%""
              style=""max-width: 600px""
            >
              <tbody>
                <tr>
                  <td
                    align=""center""
                    valign=""top""
                    style=""padding: 30px 10px""
                  ></td>
                </tr>
              </tbody>
            </table>
          </td>
        </tr>
      </tbody>
    </table>
  </body>
</html>
";
        }

        public static string AnnounceRejectRegistration(string fullname, string reason)
        {
            return @"<!doctype html>
<html>
  <head>
    <title></title>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1"" />
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"" />
    <style type=""text/css"">
      body {
        font-family: sans-serif;
      }

      body,
      table,
      td,
      a {
        -webkit-text-size-adjust: 100%;
        -ms-text-size-adjust: 100%;
      }

      table,
      td {
        mso-table-lspace: 0pt;
        mso-table-rspace: 0pt;
      }

      img {
        -ms-interpolation-mode: bicubic;
      }

      /* RESET STYLES */
      img {
        border: 0;
        height: auto;
        line-height: 100%;
        outline: none;
        text-decoration: none;
      }

      table {
        border-collapse: collapse !important;
      }

      body {
        height: 100% !important;
        margin: 0 !important;
        padding: 0 !important;
        width: 100% !important;
      }

      /* iOS BLUE LINKS */
      a[x-apple-data-detectors] {
        color: inherit !important;
        text-decoration: none !important;
        font-size: inherit !important;
        font-family: inherit !important;
        font-weight: inherit !important;
        line-height: inherit !important;
      }

      /* MOBILE STYLES */
      @media screen and (max-width: 600px) {
        h1 {
          font-size: 32px !important;
          line-height: 32px !important;
        }
      }

      /* ANDROID CENTER FIX */
      div[style*=""margin: 16px 0;""] {
        margin: 0 !important;
      }
    </style>
  </head>

  <body
    style=""
      background-color: #f4f4f4;
      margin: 0 !important;
      padding: 0 !important;
    ""
  >
    <!-- HIDDEN PREHEADER TEXT -->
    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
      <tbody>
        <tr>
          <td bgcolor=""#422A14"" align=""center"">
            <table
              border=""0""
              cellpadding=""0""
              cellspacing=""0""
              width=""100%""
              style=""max-width: 600px""
            >
              <tbody>
                <tr>
                  <td
                    align=""center""
                    valign=""top""
                    style=""padding: 30px 10px""
                  ></td>
                </tr>
              </tbody>
            </table>
          </td>
        </tr>
        <tr>
          <td bgcolor=""#422A14"" align=""center"" style=""padding: 0px 10px"">
            <table
              border=""0""
              cellpadding=""0""
              cellspacing=""0""
              width=""100%""
              style=""max-width: 800px; height: auto""
            >
              <tbody>
                <tr>
                  <td
                    bgcolor=""#ffffff""
                    align=""center""
                    valign=""top""
                    style=""
                      padding: 30px 50px;
                      border-radius: 4px 4px 0px 0px;
                      color: #000;
                      font-weight: 400;
                      text-align: left;
                    ""
                  >
                    <h1
                      style=""
                        font-weight: bolder;
                        font-size: 24px;
                        color: #422a14;
                        margin: 0px;
                        margin-bottom: 10px;
                        text-align: left;
                      ""
                    >
                      Catechist Helper
                    </h1>
                    <h1
                      style=""
                        font-size: 18px;
                        font-weight: 400;
                        margin: 0px;
                        margin-bottom: 10px;
                        color: #422a14;
                        text-align: left;
                        font-weight: 600;
                      ""
                    >
                      Thân gửi " + fullname + @"
                    </h1>
                    <p style=""margin: 0px; text-align: left; line-height: 20px"">
                      Chúng tôi cảm ơn bạn đã dành thời gian nộp hồ sơ ứng tuyển
                      cho vị trí Giáo lý viên tại Giáo xứ của chúng tôi. Sau khi
                      xem xét kỹ lưỡng, rất tiếc phải thông báo rằng hồ sơ của
                      bạn chưa phù hợp với yêu cầu của vị trí này.
                    </p>
                    <p style=""margin: 0px; text-align: left; line-height: 20px"">
                    <strong>Lý do: </strong> "+ reason + @"
                    </p>
                    <p
                      style=""
                        margin: 0px;
                        text-align: left;
                        line-height: 20px;
                        margin: 13px 0px;
                      ""
                    >
                      Chúng tôi mong muốn bạn sẽ tiếp tục quan tâm và nộp hồ sơ
                      cho các vị trí khác phù hợp hơn trong tương lai.
                    </p>
                    <p style=""margin: 0; padding: 0"">Trân trọng,</p>
                    <p style=""margin: 0; padding: 0; margin-top: 2px"">
                      Catechist Helper
                    </p>
                  </td>
                </tr>
              </tbody>
            </table>
          </td>
        </tr>
        <tr>
          <td bgcolor=""#422A14"" align=""center"">
            <table
              border=""0""
              cellpadding=""0""
              cellspacing=""0""
              width=""100%""
              style=""max-width: 600px""
            >
              <tbody>
                <tr>
                  <td
                    align=""center""
                    valign=""top""
                    style=""padding: 30px 10px""
                  ></td>
                </tr>
              </tbody>
            </table>
          </td>
        </tr>
      </tbody>
    </table>
  </body>
</html>
";
        }

        public static string AnnounceInterviewSchedule(string fullname, string time, string address)
        {
            return @"<!doctype html>
<html>
  <head>
    <title></title>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1"" />
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"" />
    <style type=""text/css"">
      body {
        font-family: sans-serif;
      }

      body,
      table,
      td,
      a {
        -webkit-text-size-adjust: 100%;
        -ms-text-size-adjust: 100%;
      }

      table,
      td {
        mso-table-lspace: 0pt;
        mso-table-rspace: 0pt;
      }

      img {
        -ms-interpolation-mode: bicubic;
      }

      /* RESET STYLES */
      img {
        border: 0;
        height: auto;
        line-height: 100%;
        outline: none;
        text-decoration: none;
      }

      table {
        border-collapse: collapse !important;
      }

      body {
        height: 100% !important;
        margin: 0 !important;
        padding: 0 !important;
        width: 100% !important;
      }

      /* iOS BLUE LINKS */
      a[x-apple-data-detectors] {
        color: inherit !important;
        text-decoration: none !important;
        font-size: inherit !important;
        font-family: inherit !important;
        font-weight: inherit !important;
        line-height: inherit !important;
      }

      /* MOBILE STYLES */
      @media screen and (max-width: 600px) {
        h1 {
          font-size: 32px !important;
          line-height: 32px !important;
        }
      }

      /* ANDROID CENTER FIX */
      div[style*=""margin: 16px 0;""] {
        margin: 0 !important;
      }
    </style>
  </head>

  <body
    style=""
      background-color: #f4f4f4;
      margin: 0 !important;
      padding: 0 !important;
    ""
  >
    <!-- HIDDEN PREHEADER TEXT -->
    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
      <tbody>
        <tr>
          <td bgcolor=""#422A14"" align=""center"">
            <table
              border=""0""
              cellpadding=""0""
              cellspacing=""0""
              width=""100%""
              style=""max-width: 600px""
            >
              <tbody>
                <tr>
                  <td
                    align=""center""
                    valign=""top""
                    style=""padding: 30px 10px""
                  ></td>
                </tr>
              </tbody>
            </table>
          </td>
        </tr>
        <tr>
          <td bgcolor=""#422A14"" align=""center"" style=""padding: 0px 10px"">
            <table
              border=""0""
              cellpadding=""0""
              cellspacing=""0""
              width=""100%""
              style=""max-width: 800px; height: auto""
            >
              <tbody>
                <tr>
                  <td
                    bgcolor=""#ffffff""
                    align=""center""
                    valign=""top""
                    style=""
                      padding: 30px 50px;
                      border-radius: 4px 4px 0px 0px;
                      color: #000;
                      font-weight: 400;
                      text-align: left;
                    ""
                  >
                    <h1
                      style=""
                        font-weight: bolder;
                        font-size: 24px;
                        color: #422a14;
                        margin: 0px;
                        margin-bottom: 10px;
                        text-align: left;
                      ""
                    >
                      Catechist Helper
                    </h1>
                    <h1
                      style=""
                        font-size: 18px;
                        font-weight: 400;
                        margin: 0px;
                        margin-bottom: 10px;
                        color: #422a14;
                        text-align: left;
                        font-weight: 600;
                      ""
                    >
                      Thân gửi " + fullname + @"
                    </h1>
                    <p style=""margin: 0px; text-align: left; line-height: 20px"">
                      Chúng tôi rất vui mừng thông báo rằng bạn đã vượt qua vòng
                      xét duyệt hồ sơ cho vị trí Giáo lý viên. Bạn được mời tham
                      gia buổi phỏng vấn với thông tin cụ thể như sau:
                    </p>
                    <ul>
                      <li style=""margin-bottom: 5px"">
                        <strong>Thời gian:</strong> "+time+ @"
                      </li>
                      <li><strong>Địa điểm:</strong> "+address+@"</li>
                    </ul>
                    <p
                      style=""
                        margin: 0px;
                        text-align: left;
                        line-height: 20px;
                        margin: 13px 0px;
                      ""
                    >
                      Chúng tôi mong được gặp bạn tại buổi phỏng vấn!
                    </p>
                    <p style=""margin: 0; padding: 0"">Trân trọng,</p>
                    <p style=""margin: 0; padding: 0; margin-top: 2px"">
                      Catechist Helper
                    </p>
                  </td>
                </tr>
              </tbody>
            </table>
          </td>
        </tr>
        <tr>
          <td bgcolor=""#422A14"" align=""center"">
            <table
              border=""0""
              cellpadding=""0""
              cellspacing=""0""
              width=""100%""
              style=""max-width: 600px""
            >
              <tbody>
                <tr>
                  <td
                    align=""center""
                    valign=""top""
                    style=""padding: 30px 10px""
                  ></td>
                </tr>
              </tbody>
            </table>
          </td>
        </tr>
      </tbody>
    </table>
  </body>
</html>
";
        }

        public static string AnnounceUpdateInterviewSchedule(string fullname, string reason, string time, string address)
        {
            return @"<!doctype html>
<html>
  <head>
    <title></title>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1"" />
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"" />
    <style type=""text/css"">
      body {
        font-family: sans-serif;
      }

      body,
      table,
      td,
      a {
        -webkit-text-size-adjust: 100%;
        -ms-text-size-adjust: 100%;
      }

      table,
      td {
        mso-table-lspace: 0pt;
        mso-table-rspace: 0pt;
      }

      img {
        -ms-interpolation-mode: bicubic;
      }

      /* RESET STYLES */
      img {
        border: 0;
        height: auto;
        line-height: 100%;
        outline: none;
        text-decoration: none;
      }

      table {
        border-collapse: collapse !important;
      }

      body {
        height: 100% !important;
        margin: 0 !important;
        padding: 0 !important;
        width: 100% !important;
      }

      /* iOS BLUE LINKS */
      a[x-apple-data-detectors] {
        color: inherit !important;
        text-decoration: none !important;
        font-size: inherit !important;
        font-family: inherit !important;
        font-weight: inherit !important;
        line-height: inherit !important;
      }

      /* MOBILE STYLES */
      @media screen and (max-width: 600px) {
        h1 {
          font-size: 32px !important;
          line-height: 32px !important;
        }
      }

      /* ANDROID CENTER FIX */
      div[style*=""margin: 16px 0;""] {
        margin: 0 !important;
      }
    </style>
  </head>

  <body
    style=""
      background-color: #f4f4f4;
      margin: 0 !important;
      padding: 0 !important;
    ""
  >
    <!-- HIDDEN PREHEADER TEXT -->
    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
      <tbody>
        <tr>
          <td bgcolor=""#422A14"" align=""center"">
            <table
              border=""0""
              cellpadding=""0""
              cellspacing=""0""
              width=""100%""
              style=""max-width: 600px""
            >
              <tbody>
                <tr>
                  <td
                    align=""center""
                    valign=""top""
                    style=""padding: 30px 10px""
                  ></td>
                </tr>
              </tbody>
            </table>
          </td>
        </tr>
        <tr>
          <td bgcolor=""#422A14"" align=""center"" style=""padding: 0px 10px"">
            <table
              border=""0""
              cellpadding=""0""
              cellspacing=""0""
              width=""100%""
              style=""max-width: 800px; height: auto""
            >
              <tbody>
                <tr>
                  <td
                    bgcolor=""#ffffff""
                    align=""center""
                    valign=""top""
                    style=""
                      padding: 30px 50px;
                      border-radius: 4px 4px 0px 0px;
                      color: #000;
                      font-weight: 400;
                      text-align: left;
                    ""
                  >
                    <h1
                      style=""
                        font-weight: bolder;
                        font-size: 24px;
                        color: #422a14;
                        margin: 0px;
                        margin-bottom: 10px;
                        text-align: left;
                      ""
                    >
                      Catechist Helper
                    </h1>
                    <h1
                      style=""
                        font-size: 18px;
                        font-weight: 400;
                        margin: 0px;
                        margin-bottom: 10px;
                        color: #422a14;
                        text-align: left;
                        font-weight: 600;
                      ""
                    >
                      Thân gửi " + fullname + @"
                    </h1>

                    <p style=""margin: 0px; text-align: left; line-height: 20px"">
                      Chúng tôi xin thông báo có sự thay đổi trong lịch phỏng
                      vấn cho vị trí Giáo lý viên của bạn. <strong>Vì lý do: </strong> " + reason + @"
                    </p>

                    <p style=""margin: 0px; text-align: left; line-height: 20px"">
                        Thông tin cập nhật như sau:
                    </p>

                    
                    <ul>
                      <li style=""margin-bottom: 5px"">
                        <strong>Thời gian:</strong> "+time+ @"
                      </li>
                      <li><strong>Địa điểm:</strong> "+address+@"</li>
                    </ul>

                    <p
                      style=""
                        margin: 0px;
                        text-align: left;
                        line-height: 20px;
                        margin: 13px 0px;
                      ""
                    >
                      Chúng tôi xin lỗi vì sự thay đổi này và mong nhận được sự
                      thông cảm của bạn về lịch phỏng vấn mới.
                    </p>
                    <p style=""margin: 0; padding: 0"">Trân trọng,</p>
                    <p style=""margin: 0; padding: 0; margin-top: 2px"">
                      Catechist Helper
                    </p>
                  </td>
                </tr>
              </tbody>
            </table>
          </td>
        </tr>
        <tr>
          <td bgcolor=""#422A14"" align=""center"">
            <table
              border=""0""
              cellpadding=""0""
              cellspacing=""0""
              width=""100%""
              style=""max-width: 600px""
            >
              <tbody>
                <tr>
                  <td
                    align=""center""
                    valign=""top""
                    style=""padding: 30px 10px""
                  ></td>
                </tr>
              </tbody>
            </table>
          </td>
        </tr>
      </tbody>
    </table>
  </body>
</html>
";
        }

        public static string AnnounceApproveInterview(string fullname)
        {
            return @"<!doctype html>
<html>
  <head>
    <title></title>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1"" />
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"" />
    <style type=""text/css"">
      body {
        font-family: sans-serif;
      }

      body,
      table,
      td,
      a {
        -webkit-text-size-adjust: 100%;
        -ms-text-size-adjust: 100%;
      }

      table,
      td {
        mso-table-lspace: 0pt;
        mso-table-rspace: 0pt;
      }

      img {
        -ms-interpolation-mode: bicubic;
      }

      /* RESET STYLES */
      img {
        border: 0;
        height: auto;
        line-height: 100%;
        outline: none;
        text-decoration: none;
      }

      table {
        border-collapse: collapse !important;
      }

      body {
        height: 100% !important;
        margin: 0 !important;
        padding: 0 !important;
        width: 100% !important;
      }

      /* iOS BLUE LINKS */
      a[x-apple-data-detectors] {
        color: inherit !important;
        text-decoration: none !important;
        font-size: inherit !important;
        font-family: inherit !important;
        font-weight: inherit !important;
        line-height: inherit !important;
      }

      /* MOBILE STYLES */
      @media screen and (max-width: 600px) {
        h1 {
          font-size: 32px !important;
          line-height: 32px !important;
        }
      }

      /* ANDROID CENTER FIX */
      div[style*=""margin: 16px 0;""] {
        margin: 0 !important;
      }
    </style>
  </head>

  <body
    style=""
      background-color: #f4f4f4;
      margin: 0 !important;
      padding: 0 !important;
    ""
  >
    <!-- HIDDEN PREHEADER TEXT -->
    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
      <tbody>
        <tr>
          <td bgcolor=""#422A14"" align=""center"">
            <table
              border=""0""
              cellpadding=""0""
              cellspacing=""0""
              width=""100%""
              style=""max-width: 600px""
            >
              <tbody>
                <tr>
                  <td
                    align=""center""
                    valign=""top""
                    style=""padding: 30px 10px""
                  ></td>
                </tr>
              </tbody>
            </table>
          </td>
        </tr>
        <tr>
          <td bgcolor=""#422A14"" align=""center"" style=""padding: 0px 10px"">
            <table
              border=""0""
              cellpadding=""0""
              cellspacing=""0""
              width=""100%""
              style=""max-width: 800px; height: auto""
            >
              <tbody>
                <tr>
                  <td
                    bgcolor=""#ffffff""
                    align=""center""
                    valign=""top""
                    style=""
                      padding: 30px 50px;
                      border-radius: 4px 4px 0px 0px;
                      color: #000;
                      font-weight: 400;
                      text-align: left;
                    ""
                  >
                    <h1
                      style=""
                        font-weight: bolder;
                        font-size: 24px;
                        color: #422a14;
                        margin: 0px;
                        margin-bottom: 10px;
                        text-align: left;
                      ""
                    >
                      Catechist Helper
                    </h1>
                    <h1
                      style=""
                        font-size: 18px;
                        font-weight: 400;
                        margin: 0px;
                        margin-bottom: 10px;
                        color: #422a14;
                        text-align: left;
                        font-weight: 600;
                      ""
                    >
                      Thân gửi " + fullname + @"
                    </h1>
                    <p style=""margin: 0px; text-align: left; line-height: 20px"">
                      Chúng tôi rất vui mừng thông báo rằng bạn đã vượt qua buổi
                      phỏng vấn và được chọn cho vị trí Giáo lý viên.
                    </p>
                    <p
                      style=""
                        margin: 0px;
                        text-align: left;
                        line-height: 20px;
                        margin: 13px 0px;
                      ""
                    >
                      Chúng tôi sẽ liên hệ với bạn sớm để thảo luận về các bước
                      tiếp theo. Xin chúc mừng và hoan nghênh bạn gia nhập giáo
                      xứ của chúng tôi!
                    </p>
                    <p style=""margin: 0; padding: 0"">Trân trọng,</p>
                    <p style=""margin: 0; padding: 0; margin-top: 2px"">
                      Catechist Helper
                    </p>
                  </td>
                </tr>
              </tbody>
            </table>
          </td>
        </tr>
        <tr>
          <td bgcolor=""#422A14"" align=""center"">
            <table
              border=""0""
              cellpadding=""0""
              cellspacing=""0""
              width=""100%""
              style=""max-width: 600px""
            >
              <tbody>
                <tr>
                  <td
                    align=""center""
                    valign=""top""
                    style=""padding: 30px 10px""
                  ></td>
                </tr>
              </tbody>
            </table>
          </td>
        </tr>
      </tbody>
    </table>
  </body>
</html>
";
        }

        public static string AnnounceRejectInterview(string fullname)
        {
            return @"<!doctype html>
<html>
  <head>
    <title></title>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1"" />
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"" />
    <style type=""text/css"">
      body {
        font-family: sans-serif;
      }

      body,
      table,
      td,
      a {
        -webkit-text-size-adjust: 100%;
        -ms-text-size-adjust: 100%;
      }

      table,
      td {
        mso-table-lspace: 0pt;
        mso-table-rspace: 0pt;
      }

      img {
        -ms-interpolation-mode: bicubic;
      }

      /* RESET STYLES */
      img {
        border: 0;
        height: auto;
        line-height: 100%;
        outline: none;
        text-decoration: none;
      }

      table {
        border-collapse: collapse !important;
      }

      body {
        height: 100% !important;
        margin: 0 !important;
        padding: 0 !important;
        width: 100% !important;
      }

      /* iOS BLUE LINKS */
      a[x-apple-data-detectors] {
        color: inherit !important;
        text-decoration: none !important;
        font-size: inherit !important;
        font-family: inherit !important;
        font-weight: inherit !important;
        line-height: inherit !important;
      }

      /* MOBILE STYLES */
      @media screen and (max-width: 600px) {
        h1 {
          font-size: 32px !important;
          line-height: 32px !important;
        }
      }

      /* ANDROID CENTER FIX */
      div[style*=""margin: 16px 0;""] {
        margin: 0 !important;
      }
    </style>
  </head>

  <body
    style=""
      background-color: #f4f4f4;
      margin: 0 !important;
      padding: 0 !important;
    ""
  >
    <!-- HIDDEN PREHEADER TEXT -->
    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
      <tbody>
        <tr>
          <td bgcolor=""#422A14"" align=""center"">
            <table
              border=""0""
              cellpadding=""0""
              cellspacing=""0""
              width=""100%""
              style=""max-width: 600px""
            >
              <tbody>
                <tr>
                  <td
                    align=""center""
                    valign=""top""
                    style=""padding: 30px 10px""
                  ></td>
                </tr>
              </tbody>
            </table>
          </td>
        </tr>
        <tr>
          <td bgcolor=""#422A14"" align=""center"" style=""padding: 0px 10px"">
            <table
              border=""0""
              cellpadding=""0""
              cellspacing=""0""
              width=""100%""
              style=""max-width: 800px; height: auto""
            >
              <tbody>
                <tr>
                  <td
                    bgcolor=""#ffffff""
                    align=""center""
                    valign=""top""
                    style=""
                      padding: 30px 50px;
                      border-radius: 4px 4px 0px 0px;
                      color: #000;
                      font-weight: 400;
                      text-align: left;
                    ""
                  >
                    <h1
                      style=""
                        font-weight: bolder;
                        font-size: 24px;
                        color: #422a14;
                        margin: 0px;
                        margin-bottom: 10px;
                        text-align: left;
                      ""
                    >
                      Catechist Helper
                    </h1>
                    <h1
                      style=""
                        font-size: 18px;
                        font-weight: 400;
                        margin: 0px;
                        margin-bottom: 10px;
                        color: #422a14;
                        text-align: left;
                        font-weight: 600;
                      ""
                    >
                      Thân gửi " + fullname + @"
                    </h1>
                    <p style=""margin: 0px; text-align: left; line-height: 20px"">
                      Cảm ơn bạn đã tham gia buổi phỏng vấn cho vị trí Giáo lý
                      viên. Sau khi xem xét kỹ lưỡng, chúng tôi rất tiếc phải
                      thông báo rằng bạn không được chọn cho vị trí này.
                    </p>
                    <p
                      style=""
                        margin: 0px;
                        text-align: left;
                        line-height: 20px;
                        margin: 13px 0px;
                      ""
                    >
                      Chúng tôi rất trân trọng thời gian và công sức bạn đã bỏ
                      ra và hy vọng sẽ có cơ hội hợp tác với bạn trong tương
                      lai.
                    </p>
                    <p
                      style=""
                        margin: 0px;
                        text-align: left;
                        line-height: 20px;
                        margin: 13px 0px;
                      ""
                    >
                      Chúc bạn thành công trong những cơ hội khác!
                    </p>
                    <p style=""margin: 0; padding: 0"">Trân trọng,</p>
                    <p style=""margin: 0; padding: 0; margin-top: 2px"">
                      Catechist Helper
                    </p>
                  </td>
                </tr>
              </tbody>
            </table>
          </td>
        </tr>
        <tr>
          <td bgcolor=""#422A14"" align=""center"">
            <table
              border=""0""
              cellpadding=""0""
              cellspacing=""0""
              width=""100%""
              style=""max-width: 600px""
            >
              <tbody>
                <tr>
                  <td
                    align=""center""
                    valign=""top""
                    style=""padding: 30px 10px""
                  ></td>
                </tr>
              </tbody>
            </table>
          </td>
        </tr>
      </tbody>
    </table>
  </body>
</html>
";
        }

        public static string AnnounceAccountInformation(string fullname, string email, string password)
        {
            return @"<!doctype html>
<html>
  <head>
    <title></title>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1"" />
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"" />
    <style type=""text/css"">
      body {
        font-family: sans-serif;
      }

      body,
      table,
      td,
      a {
        -webkit-text-size-adjust: 100%;
        -ms-text-size-adjust: 100%;
      }

      table,
      td {
        mso-table-lspace: 0pt;
        mso-table-rspace: 0pt;
      }

      img {
        -ms-interpolation-mode: bicubic;
      }

      /* RESET STYLES */
      img {
        border: 0;
        height: auto;
        line-height: 100%;
        outline: none;
        text-decoration: none;
      }

      table {
        border-collapse: collapse !important;
      }

      body {
        height: 100% !important;
        margin: 0 !important;
        padding: 0 !important;
        width: 100% !important;
      }

      /* iOS BLUE LINKS */
      a[x-apple-data-detectors] {
        color: inherit !important;
        text-decoration: none !important;
        font-size: inherit !important;
        font-family: inherit !important;
        font-weight: inherit !important;
        line-height: inherit !important;
      }

      /* MOBILE STYLES */
      @media screen and (max-width: 600px) {
        h1 {
          font-size: 32px !important;
          line-height: 32px !important;
        }
      }

      /* ANDROID CENTER FIX */
      div[style*=""margin: 16px 0;""] {
        margin: 0 !important;
      }
    </style>
  </head>

  <body
    style=""
      background-color: #f4f4f4;
      margin: 0 !important;
      padding: 0 !important;
    ""
  >
    <!-- HIDDEN PREHEADER TEXT -->
    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
      <tbody>
        <tr>
          <td bgcolor=""#422A14"" align=""center"">
            <table
              border=""0""
              cellpadding=""0""
              cellspacing=""0""
              width=""100%""
              style=""max-width: 600px""
            >
              <tbody>
                <tr>
                  <td
                    align=""center""
                    valign=""top""
                    style=""padding: 30px 10px""
                  ></td>
                </tr>
              </tbody>
            </table>
          </td>
        </tr>
        <tr>
          <td bgcolor=""#422A14"" align=""center"" style=""padding: 0px 10px"">
            <table
              border=""0""
              cellpadding=""0""
              cellspacing=""0""
              width=""100%""
              style=""max-width: 800px; height: auto""
            >
              <tbody>
                <tr>
                  <td
                    bgcolor=""#ffffff""
                    align=""center""
                    valign=""top""
                    style=""
                      padding: 30px 50px;
                      border-radius: 4px 4px 0px 0px;
                      color: #000;
                      font-weight: 400;
                      text-align: left;
                    ""
                  >
                    <h1
                      style=""
                        font-weight: bolder;
                        font-size: 24px;
                        color: #422a14;
                        margin: 0px;
                        margin-bottom: 10px;
                        text-align: left;
                      ""
                    >
                      Catechist Helper
                    </h1>
                    <h1
                      style=""
                        font-size: 18px;
                        font-weight: 400;
                        margin: 0px;
                        margin-bottom: 10px;
                        color: #422a14;
                        text-align: left;
                        font-weight: 600;
                      ""
                    >
                      Thân gửi " + fullname + @"
                    </h1>
                    <p style=""margin: 0px; text-align: left; line-height: 20px"">
                      Sau đây là thông tin của tài khoản đăng nhập vào hệ thống của bạn:
                    </p>
                    <ul>
                      <li style=""margin-bottom: 5px"">
                        <strong>Email đăng nhập:</strong> " + email + @"
                      </li>
                      <li><strong>Mật khẩu đăng nhập:</strong> " + password + @"</li>
                    </ul>
                    <p style=""margin: 0; padding: 0"">Trân trọng,</p>
                    <p style=""margin: 0; padding: 0; margin-top: 2px"">
                      Catechist Helper
                    </p>
                  </td>
                </tr>
              </tbody>
            </table>
          </td>
        </tr>
        <tr>
          <td bgcolor=""#422A14"" align=""center"">
            <table
              border=""0""
              cellpadding=""0""
              cellspacing=""0""
              width=""100%""
              style=""max-width: 600px""
            >
              <tbody>
                <tr>
                  <td
                    align=""center""
                    valign=""top""
                    style=""padding: 30px 10px""
                  ></td>
                </tr>
              </tbody>
            </table>
          </td>
        </tr>
      </tbody>
    </table>
  </body>
</html>
";
        }


    }
}

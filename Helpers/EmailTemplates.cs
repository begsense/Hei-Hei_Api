namespace Hei_Hei_Api.Helpers;

public static class EmailTemplates
{
    private static string Wrap(string name, string title, string bodyContent) => $"""
        <!DOCTYPE html>
        <html>
        <body style="margin:0;padding:0;background:#f4f4f4;font-family:Arial,sans-serif;">
          <table width="100%" cellpadding="0" cellspacing="0">
            <tr>
              <td align="center" style="padding:40px 0;">
                <table width="600" cellpadding="0" cellspacing="0" style="background:#ffffff;border-radius:8px;overflow:hidden;">
                  
                  <!-- Header -->
                  <tr>
                    <td style="background:#4F46E5;padding:30px;text-align:center;">
                      <h1 style="color:#ffffff;margin:0;font-size:24px;">Hei Hei 🎉</h1>
                    </td>
                  </tr>

                  <!-- Body -->
                  <tr>
                    <td style="padding:40px 30px;">
                      <p style="color:#333;font-size:16px;margin:0 0 16px;">Hi <strong>{name}</strong>,</p>
                      <h2 style="color:#4F46E5;font-size:20px;margin:0 0 16px;">{title}</h2>
                      {bodyContent}
                    </td>
                  </tr>

                  <!-- Footer -->
                  <tr>
                    <td style="background:#f4f4f4;padding:20px;text-align:center;">
                      <p style="color:#999;font-size:12px;margin:0;">© {DateTime.UtcNow.Year} Hei Hei. All rights reserved.</p>
                    </td>
                  </tr>

                </table>
              </td>
            </tr>
          </table>
        </body>
        </html>
        """;

    public static string VerificationCode(string name, string code) =>
        Wrap(name, "Verify your email address", $"""
            <p style="color:#555;font-size:15px;line-height:1.6;">
                Thanks for signing up! Please use the code below to verify your email address.
                This code expires in <strong>15 minutes</strong>.
            </p>
            <div style="text-align:center;margin:30px 0;">
                <span style="background:#4F46E5;color:#ffffff;font-size:32px;font-weight:bold;
                             letter-spacing:8px;padding:16px 32px;border-radius:8px;">
                    {code}
                </span>
            </div>
            <p style="color:#999;font-size:13px;">
                If you did not create an account, you can safely ignore this email.
            </p>
        """);

    public static string Welcome(string name) =>
        Wrap(name, "Welcome aboard! 🎊", """
            <p style="color:#555;font-size:15px;line-height:1.6;">
                Your account has been created successfully. We're thrilled to have you with us!
            </p>
            <p style="color:#555;font-size:15px;line-height:1.6;">
                You can now log in and start exploring everything Hei Hei has to offer.
            </p>
        """);

    public static string PasswordChanged(string name) =>
        Wrap(name, "Your password was changed", """
            <p style="color:#555;font-size:15px;line-height:1.6;">
                Your account password was recently updated.
            </p>
            <p style="color:#555;font-size:15px;line-height:1.6;">
                If you made this change, no action is needed. If you did <strong>not</strong> request this change, 
                please contact our support team immediately.
            </p>
        """);

    public static string AccountDeleted(string name) =>
        Wrap(name, "Your account has been deleted", """
            <p style="color:#555;font-size:15px;line-height:1.6;">
                Your account has been permanently deleted. We're sorry to see you go.
            </p>
            <p style="color:#555;font-size:15px;line-height:1.6;">
                If this was a mistake or you have any questions, please reach out to our support team.
            </p>
        """);
}

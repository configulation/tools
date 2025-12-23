using System;
using System.Windows.Forms;
using Sunny.UI;

namespace WinFormsApp1
{
	public class FormWrapperPage : UIPage
	{
		private readonly Form innerForm;

		public FormWrapperPage(Form form)
		{
			if (form == null) throw new ArgumentNullException(nameof(form));
			innerForm = form;
			this.Text = string.IsNullOrWhiteSpace(form.Text) ? form.GetType().Name : form.Text;
			this.AllowShowTitle = false;
			this.Padding = new Padding(0);
			this.Load += FormWrapperPage_Load;
		}

		private void FormWrapperPage_Load(object sender, EventArgs e)
		{
			innerForm.TopLevel = false;
			innerForm.FormBorderStyle = FormBorderStyle.None;
			innerForm.Dock = DockStyle.Fill;
			Controls.Add(innerForm);
			innerForm.Show();
		}
	}
} 
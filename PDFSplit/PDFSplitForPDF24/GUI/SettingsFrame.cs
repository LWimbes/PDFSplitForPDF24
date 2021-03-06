﻿using PDFSplitForPDF24.ProgramSettings;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PDFSplitForPDF24 {
    public partial class SettingsFrame : Form {
        public SettingsFrame() {
            InitializeComponent();

            unitComboBox.Items.Add(QuantityUnit.MB);
            unitComboBox.Items.Add(QuantityUnit.MiB);
            unitComboBox.Items.Add(QuantityUnit.Seiten);
            unitComboBox.SelectedItem = Program.Sett.QUnit;

            sizeNumericTextBox.Text = Program.Sett.Size.ToString();

            cacheTextBox.Text = Program.Sett.Cache;

            safeButton.Click += new EventHandler(SafeButton_Click);
        }

        private void SafeButton_Click(object sender, EventArgs e) {

            // Check whether a Item is selected
            if (unitComboBox.SelectedItem != null) {
                Program.Sett.QUnit = (QuantityUnit)unitComboBox.SelectedItem;
                Program.Sett.SafeToFile();
            } else {
                MessageBox.Show("Die Einheit ist ungültig!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            // Check whether a valid number was entered
            if (sizeNumericTextBox.Text != "" && int.TryParse(sizeNumericTextBox.Text, out int size) && size > 0) {
                Program.Sett.Size = size;
                Program.Sett.SafeToFile();
            } else {
                MessageBox.Show("Die Ganzzahl muss größer 0 sein!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            // Check Cache Path

            // Check if the path is empty
            if (cacheTextBox.Text == "") {
                MessageBox.Show("Der Pfad für den Chache darf nicht leer sein!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if path contains multiple %work% or %work% and %install% or %work% not at the start
            if (cacheTextBox.Text.Contains("%work%")) {
                if (cacheTextBox.Text.Contains("%install%")) {
                    MessageBox.Show("Sie müssen sich für %work% oder %install% entscheiden!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                } else if (!cacheTextBox.Text.StartsWith("%work%")) {
                    MessageBox.Show("%work% darf nur am Anfang stehen!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                } else if (Regex.Matches(cacheTextBox.Text, "%work%").Count > 1) {
                    MessageBox.Show("%work% darf nur einmal vorkommen!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // Check if path contains multiple %install% or %install% and %work% or %install% not at the start
            if (cacheTextBox.Text.Contains("%install%")) {
                if (cacheTextBox.Text.Contains("%work%")) {
                    MessageBox.Show("Sie müssen sich für %work% oder %install% entscheiden!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                } else if (!cacheTextBox.Text.StartsWith("%install%")) {
                    MessageBox.Show("%install% darf nur am Anfang stehen!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                } else if (Regex.Matches(cacheTextBox.Text, "%install%").Count > 1) {
                    MessageBox.Show("%install% darf nur einmal vorkommen!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // Check if path is okay (only if not %work%)
            if (!cacheTextBox.Text.Contains("%work%") && !Directory.Exists(cacheTextBox.Text.Replace("%install%", Path.GetDirectoryName(Application.ExecutablePath)))) {
                MessageBox.Show("Der Ordner, in welchem der Cache angelegt werden soll, existiert nicht!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Program.Sett.Cache = cacheTextBox.Text;
            Program.Sett.SafeToFile();

            this.Close();
        }
    }
}

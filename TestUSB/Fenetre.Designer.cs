using System;
using System.ComponentModel;

namespace GeCoSwell
{
    partial class Fenetre1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Fenetre1));
            this.b_Connection = new System.Windows.Forms.Button();
            this.b_Envoi = new System.Windows.Forms.Button();
            this.fichierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sauvegarderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ouvrirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sauvegarderToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.quitterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.communicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.déconnectéToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.envoiDesParamètresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.réceptionDesParamètresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.relancerLeServeurToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outilsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.générationTableSinusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.débugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.àProposToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.b_Reception = new System.Windows.Forms.Button();
            this.l_info_connection = new System.Windows.Forms.Label();
            this.b_lancer_serveur = new System.Windows.Forms.Button();
            this.b_com_multi_niv = new System.Windows.Forms.Button();
            this.cmb_mode_puissance = new System.Windows.Forms.ComboBox();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm_ouvrir = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm_sauvegarde = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.tsm_quit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm_connect = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm_deconnecte = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.tsm_send = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm_reception = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.tsm_serveur = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem13 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm_option = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.tsm_gen_sinus = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm_debug = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm_log = new System.Windows.Forms.ToolStripMenuItem();
            this.superUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem18 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm_apropos = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.chb_mode_onduleur = new System.Windows.Forms.CheckBox();
            this.bt_com = new System.Windows.Forms.Button();
            this.chb_mode_multiniv = new System.Windows.Forms.CheckBox();
            this.gb_mode_fonctionnement = new System.Windows.Forms.GroupBox();
            this.chb_surmodulation = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button3 = new System.Windows.Forms.Button();
            this.panel_modulaire = new System.Windows.Forms.Panel();
            this.menuStrip.SuspendLayout();
            this.gb_mode_fonctionnement.SuspendLayout();
            this.panel_modulaire.SuspendLayout();
            this.SuspendLayout();
            // 
            // b_Connection
            // 
            this.b_Connection.Enabled = false;
            this.b_Connection.Image = global::GeCoSwell.Properties.Resources.icons8_connected_16;
            this.b_Connection.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.b_Connection.Location = new System.Drawing.Point(103, 27);
            this.b_Connection.Name = "b_Connection";
            this.b_Connection.Size = new System.Drawing.Size(85, 44);
            this.b_Connection.TabIndex = 2;
            this.b_Connection.Text = "Connection";
            this.b_Connection.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.b_Connection.UseVisualStyleBackColor = true;
            this.b_Connection.Click += new System.EventHandler(this.B_Connection_serveur_Click);
            // 
            // b_Envoi
            // 
            this.b_Envoi.Enabled = false;
            this.b_Envoi.Image = global::GeCoSwell.Properties.Resources.icons8_send_16;
            this.b_Envoi.Location = new System.Drawing.Point(194, 27);
            this.b_Envoi.Name = "b_Envoi";
            this.b_Envoi.Size = new System.Drawing.Size(85, 44);
            this.b_Envoi.TabIndex = 3;
            this.b_Envoi.Text = "Envoi";
            this.b_Envoi.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.b_Envoi.UseVisualStyleBackColor = true;
            this.b_Envoi.Click += new System.EventHandler(this.B_Envoi_donné_Click);
            // 
            // fichierToolStripMenuItem
            // 
            this.fichierToolStripMenuItem.Name = "fichierToolStripMenuItem";
            this.fichierToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.fichierToolStripMenuItem.Text = "&Fichier";
            // 
            // sauvegarderToolStripMenuItem
            // 
            this.sauvegarderToolStripMenuItem.Enabled = false;
            this.sauvegarderToolStripMenuItem.Name = "sauvegarderToolStripMenuItem";
            this.sauvegarderToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.sauvegarderToolStripMenuItem.Text = "&Nouveau";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // ouvrirToolStripMenuItem
            // 
            this.ouvrirToolStripMenuItem.Name = "ouvrirToolStripMenuItem";
            this.ouvrirToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ouvrirToolStripMenuItem.Text = "&Ouvrir";
            this.ouvrirToolStripMenuItem.Click += new System.EventHandler(this.ChargementToolStripMenuItem1_Click);
            // 
            // sauvegarderToolStripMenuItem1
            // 
            this.sauvegarderToolStripMenuItem1.Name = "sauvegarderToolStripMenuItem1";
            this.sauvegarderToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.sauvegarderToolStripMenuItem1.Text = "&Sauvegarder";
            this.sauvegarderToolStripMenuItem1.Click += new System.EventHandler(this.SauvegarderToolStripMenuItem1_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(177, 6);
            // 
            // quitterToolStripMenuItem
            // 
            this.quitterToolStripMenuItem.Name = "quitterToolStripMenuItem";
            this.quitterToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.quitterToolStripMenuItem.Text = "&Quitter";
            this.quitterToolStripMenuItem.Click += new System.EventHandler(this.QuitterToolStripMenuItem_Click);
            // 
            // communicationToolStripMenuItem
            // 
            this.communicationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectionToolStripMenuItem,
            this.déconnectéToolStripMenuItem,
            this.toolStripSeparator1,
            this.envoiDesParamètresToolStripMenuItem,
            this.réceptionDesParamètresToolStripMenuItem,
            this.toolStripSeparator4,
            this.relancerLeServeurToolStripMenuItem});
            this.communicationToolStripMenuItem.Name = "communicationToolStripMenuItem";
            this.communicationToolStripMenuItem.Size = new System.Drawing.Size(106, 20);
            this.communicationToolStripMenuItem.Text = "&Communication";
            // 
            // connectionToolStripMenuItem
            // 
            this.connectionToolStripMenuItem.Enabled = false;
            this.connectionToolStripMenuItem.Name = "connectionToolStripMenuItem";
            this.connectionToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.connectionToolStripMenuItem.Text = "&Connection";
            this.connectionToolStripMenuItem.Click += new System.EventHandler(this.B_Connection_serveur_Click);
            // 
            // déconnectéToolStripMenuItem
            // 
            this.déconnectéToolStripMenuItem.Enabled = false;
            this.déconnectéToolStripMenuItem.Name = "déconnectéToolStripMenuItem";
            this.déconnectéToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.déconnectéToolStripMenuItem.Text = "&Déconnection";
            this.déconnectéToolStripMenuItem.Click += new System.EventHandler(this.Tsm_deco_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(207, 6);
            // 
            // envoiDesParamètresToolStripMenuItem
            // 
            this.envoiDesParamètresToolStripMenuItem.Enabled = false;
            this.envoiDesParamètresToolStripMenuItem.Name = "envoiDesParamètresToolStripMenuItem";
            this.envoiDesParamètresToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.envoiDesParamètresToolStripMenuItem.Text = "&Envoi des paramètres";
            this.envoiDesParamètresToolStripMenuItem.Click += new System.EventHandler(this.B_Envoi_donné_Click);
            // 
            // réceptionDesParamètresToolStripMenuItem
            // 
            this.réceptionDesParamètresToolStripMenuItem.Enabled = false;
            this.réceptionDesParamètresToolStripMenuItem.Name = "réceptionDesParamètresToolStripMenuItem";
            this.réceptionDesParamètresToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.réceptionDesParamètresToolStripMenuItem.Text = "&Réception des paramètres";
            this.réceptionDesParamètresToolStripMenuItem.Click += new System.EventHandler(this.B_Reception);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(207, 6);
            // 
            // relancerLeServeurToolStripMenuItem
            // 
            this.relancerLeServeurToolStripMenuItem.Enabled = false;
            this.relancerLeServeurToolStripMenuItem.Name = "relancerLeServeurToolStripMenuItem";
            this.relancerLeServeurToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.relancerLeServeurToolStripMenuItem.Text = "Relancer le &Serveur";
            this.relancerLeServeurToolStripMenuItem.Click += new System.EventHandler(this.B_lancer_serveur_Click);
            // 
            // outilsToolStripMenuItem
            // 
            this.outilsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionToolStripMenuItem,
            this.toolStripSeparator5,
            this.générationTableSinusToolStripMenuItem});
            this.outilsToolStripMenuItem.Name = "outilsToolStripMenuItem";
            this.outilsToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.outilsToolStripMenuItem.Text = "&Outils";
            // 
            // optionToolStripMenuItem
            // 
            this.optionToolStripMenuItem.Name = "optionToolStripMenuItem";
            this.optionToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.optionToolStripMenuItem.Text = "&Option";
            this.optionToolStripMenuItem.Click += new System.EventHandler(this.Tsm_option_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(188, 6);
            // 
            // générationTableSinusToolStripMenuItem
            // 
            this.générationTableSinusToolStripMenuItem.Name = "générationTableSinusToolStripMenuItem";
            this.générationTableSinusToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.générationTableSinusToolStripMenuItem.Text = "Génération table sinus";
            this.générationTableSinusToolStripMenuItem.Click += new System.EventHandler(this.Open_Gene_sinusMenuItem_Click);
            // 
            // débugToolStripMenuItem
            // 
            this.débugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logToolStripMenuItem});
            this.débugToolStripMenuItem.Name = "débugToolStripMenuItem";
            this.débugToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.débugToolStripMenuItem.Text = "&Débug";
            // 
            // logToolStripMenuItem
            // 
            this.logToolStripMenuItem.Name = "logToolStripMenuItem";
            this.logToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.logToolStripMenuItem.Text = "&Log";
            this.logToolStripMenuItem.Click += new System.EventHandler(this.B_open_log_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.àProposToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(24, 20);
            this.toolStripMenuItem1.Text = "?";
            // 
            // àProposToolStripMenuItem
            // 
            this.àProposToolStripMenuItem.Name = "àProposToolStripMenuItem";
            this.àProposToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.àProposToolStripMenuItem.Text = "à propos";
            this.àProposToolStripMenuItem.Click += new System.EventHandler(this.AProposToolStripMenuItem_Click);
            // 
            // b_Reception
            // 
            this.b_Reception.Enabled = false;
            this.b_Reception.Image = global::GeCoSwell.Properties.Resources.icons8_received_16;
            this.b_Reception.Location = new System.Drawing.Point(285, 27);
            this.b_Reception.Name = "b_Reception";
            this.b_Reception.Size = new System.Drawing.Size(85, 44);
            this.b_Reception.TabIndex = 4;
            this.b_Reception.Text = "Réception";
            this.b_Reception.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.b_Reception.UseVisualStyleBackColor = true;
            this.b_Reception.Click += new System.EventHandler(this.B_Reception);
            // 
            // l_info_connection
            // 
            this.l_info_connection.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.l_info_connection.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_info_connection.ForeColor = System.Drawing.SystemColors.ControlText;
            this.l_info_connection.Location = new System.Drawing.Point(377, 27);
            this.l_info_connection.Name = "l_info_connection";
            this.l_info_connection.Size = new System.Drawing.Size(391, 44);
            this.l_info_connection.TabIndex = 10;
            this.l_info_connection.Text = "Serveur lancé. Ne fermé pas la fenètre DOS.";
            // 
            // b_lancer_serveur
            // 
            this.b_lancer_serveur.Enabled = false;
            this.b_lancer_serveur.Location = new System.Drawing.Point(12, 27);
            this.b_lancer_serveur.Name = "b_lancer_serveur";
            this.b_lancer_serveur.Size = new System.Drawing.Size(85, 44);
            this.b_lancer_serveur.TabIndex = 1;
            this.b_lancer_serveur.Text = "Lancer le serveur";
            this.b_lancer_serveur.UseVisualStyleBackColor = true;
            this.b_lancer_serveur.Click += new System.EventHandler(this.B_lancer_serveur_Click);
            // 
            // b_com_multi_niv
            // 
            this.b_com_multi_niv.Location = new System.Drawing.Point(945, 30);
            this.b_com_multi_niv.Name = "b_com_multi_niv";
            this.b_com_multi_niv.Size = new System.Drawing.Size(149, 39);
            this.b_com_multi_niv.TabIndex = 1203;
            this.b_com_multi_niv.Text = "Commande entrelacée multiphasé";
            this.b_com_multi_niv.UseVisualStyleBackColor = true;
            // 
            // cmb_mode_puissance
            // 
            this.cmb_mode_puissance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_mode_puissance.FormattingEnabled = true;
            this.cmb_mode_puissance.Items.AddRange(new object[] {
            "Boucle ouverte",
            "Boucle fermé",
            "Opposition",
            "Mode Triphasé",
            "Complet"});
            this.cmb_mode_puissance.Location = new System.Drawing.Point(13, 19);
            this.cmb_mode_puissance.Name = "cmb_mode_puissance";
            this.cmb_mode_puissance.Size = new System.Drawing.Size(112, 21);
            this.cmb_mode_puissance.TabIndex = 354;
            this.cmb_mode_puissance.SelectedIndexChanged += new System.EventHandler(this.Cmb_mode_puissance_SelectedIndexChanged);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsm_ouvrir,
            this.tsm_sauvegarde,
            this.toolStripSeparator7,
            this.tsm_quit});
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(54, 20);
            this.toolStripMenuItem2.Text = "&Fichier";
            // 
            // tsm_ouvrir
            // 
            this.tsm_ouvrir.Image = global::GeCoSwell.Properties.Resources.icons8_open_50;
            this.tsm_ouvrir.Name = "tsm_ouvrir";
            this.tsm_ouvrir.Size = new System.Drawing.Size(139, 22);
            this.tsm_ouvrir.Text = "&Ouvrir";
            this.tsm_ouvrir.Click += new System.EventHandler(this.ChargementToolStripMenuItem1_Click);
            // 
            // tsm_sauvegarde
            // 
            this.tsm_sauvegarde.Image = global::GeCoSwell.Properties.Resources.icons8_save_50;
            this.tsm_sauvegarde.Name = "tsm_sauvegarde";
            this.tsm_sauvegarde.Size = new System.Drawing.Size(139, 22);
            this.tsm_sauvegarde.Text = "&Sauvegarder";
            this.tsm_sauvegarde.Click += new System.EventHandler(this.SauvegarderToolStripMenuItem1_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(136, 6);
            // 
            // tsm_quit
            // 
            this.tsm_quit.Image = global::GeCoSwell.Properties.Resources.icons8_exit_sign_40;
            this.tsm_quit.Name = "tsm_quit";
            this.tsm_quit.Size = new System.Drawing.Size(139, 22);
            this.tsm_quit.Text = "&Quitter";
            this.tsm_quit.Click += new System.EventHandler(this.QuitterToolStripMenuItem_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsm_connect,
            this.tsm_deconnecte,
            this.toolStripSeparator8,
            this.tsm_send,
            this.tsm_reception,
            this.toolStripSeparator9,
            this.tsm_serveur});
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(106, 20);
            this.toolStripMenuItem7.Text = "&Communication";
            // 
            // tsm_connect
            // 
            this.tsm_connect.Enabled = false;
            this.tsm_connect.Image = global::GeCoSwell.Properties.Resources.icons8_connected_16;
            this.tsm_connect.Name = "tsm_connect";
            this.tsm_connect.Size = new System.Drawing.Size(210, 22);
            this.tsm_connect.Text = "&Connection";
            this.tsm_connect.Click += new System.EventHandler(this.B_Connection_serveur_Click);
            // 
            // tsm_deconnecte
            // 
            this.tsm_deconnecte.Enabled = false;
            this.tsm_deconnecte.Image = global::GeCoSwell.Properties.Resources.icons8_disconnected_16;
            this.tsm_deconnecte.Name = "tsm_deconnecte";
            this.tsm_deconnecte.Size = new System.Drawing.Size(210, 22);
            this.tsm_deconnecte.Text = "&Déconnection";
            this.tsm_deconnecte.Click += new System.EventHandler(this.Tsm_deco_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(207, 6);
            // 
            // tsm_send
            // 
            this.tsm_send.Enabled = false;
            this.tsm_send.Image = global::GeCoSwell.Properties.Resources.icons8_send_16;
            this.tsm_send.Name = "tsm_send";
            this.tsm_send.Size = new System.Drawing.Size(210, 22);
            this.tsm_send.Text = "&Envoi des paramètres";
            this.tsm_send.Click += new System.EventHandler(this.B_Envoi_donné_Click);
            // 
            // tsm_reception
            // 
            this.tsm_reception.Enabled = false;
            this.tsm_reception.Image = global::GeCoSwell.Properties.Resources.icons8_received_16;
            this.tsm_reception.Name = "tsm_reception";
            this.tsm_reception.Size = new System.Drawing.Size(210, 22);
            this.tsm_reception.Text = "&Réception des paramètres";
            this.tsm_reception.Click += new System.EventHandler(this.B_Reception);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(207, 6);
            // 
            // tsm_serveur
            // 
            this.tsm_serveur.Image = ((System.Drawing.Image)(resources.GetObject("tsm_serveur.Image")));
            this.tsm_serveur.Name = "tsm_serveur";
            this.tsm_serveur.Size = new System.Drawing.Size(210, 22);
            this.tsm_serveur.Text = "Relancer le &Serveur";
            this.tsm_serveur.Click += new System.EventHandler(this.B_lancer_serveur_Click);
            // 
            // toolStripMenuItem13
            // 
            this.toolStripMenuItem13.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsm_option,
            this.toolStripSeparator10,
            this.tsm_gen_sinus});
            this.toolStripMenuItem13.Name = "toolStripMenuItem13";
            this.toolStripMenuItem13.Size = new System.Drawing.Size(50, 20);
            this.toolStripMenuItem13.Text = "&Outils";
            // 
            // tsm_option
            // 
            this.tsm_option.Image = global::GeCoSwell.Properties.Resources.icons8_automatic_16;
            this.tsm_option.Name = "tsm_option";
            this.tsm_option.Size = new System.Drawing.Size(191, 22);
            this.tsm_option.Text = "&Option";
            this.tsm_option.Click += new System.EventHandler(this.Tsm_option_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(188, 6);
            // 
            // tsm_gen_sinus
            // 
            this.tsm_gen_sinus.Image = global::GeCoSwell.Properties.Resources.sinus_logo;
            this.tsm_gen_sinus.Name = "tsm_gen_sinus";
            this.tsm_gen_sinus.Size = new System.Drawing.Size(191, 22);
            this.tsm_gen_sinus.Text = "Génération table sinus";
            this.tsm_gen_sinus.Click += new System.EventHandler(this.Open_Gene_sinusMenuItem_Click);
            // 
            // tsm_debug
            // 
            this.tsm_debug.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsm_log,
            this.superUserToolStripMenuItem});
            this.tsm_debug.Name = "tsm_debug";
            this.tsm_debug.Size = new System.Drawing.Size(54, 20);
            this.tsm_debug.Text = "&Débug";
            // 
            // tsm_log
            // 
            this.tsm_log.Image = global::GeCoSwell.Properties.Resources.icons8_edit_property_16;
            this.tsm_log.Name = "tsm_log";
            this.tsm_log.Size = new System.Drawing.Size(127, 22);
            this.tsm_log.Text = "&Log";
            this.tsm_log.Click += new System.EventHandler(this.B_open_log_Click);
            // 
            // superUserToolStripMenuItem
            // 
            this.superUserToolStripMenuItem.Name = "superUserToolStripMenuItem";
            this.superUserToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.superUserToolStripMenuItem.Text = "SuperUser";
            this.superUserToolStripMenuItem.Click += new System.EventHandler(this.SuperUserToolStripMenuItem_Click);
            // 
            // toolStripMenuItem18
            // 
            this.toolStripMenuItem18.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsm_apropos});
            this.toolStripMenuItem18.Name = "toolStripMenuItem18";
            this.toolStripMenuItem18.Size = new System.Drawing.Size(24, 20);
            this.toolStripMenuItem18.Text = "?";
            // 
            // tsm_apropos
            // 
            this.tsm_apropos.Image = global::GeCoSwell.Properties.Resources.icons8_information_16;
            this.tsm_apropos.Name = "tsm_apropos";
            this.tsm_apropos.Size = new System.Drawing.Size(120, 22);
            this.tsm_apropos.Text = "à propos";
            this.tsm_apropos.Click += new System.EventHandler(this.AProposToolStripMenuItem_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem7,
            this.toolStripMenuItem13,
            this.tsm_debug,
            this.toolStripMenuItem18});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip.Size = new System.Drawing.Size(1362, 24);
            this.menuStrip.TabIndex = 31;
            this.menuStrip.Text = "menuStrip";
            // 
            // chb_mode_onduleur
            // 
            this.chb_mode_onduleur.AutoSize = true;
            this.chb_mode_onduleur.Location = new System.Drawing.Point(13, 46);
            this.chb_mode_onduleur.Name = "chb_mode_onduleur";
            this.chb_mode_onduleur.Size = new System.Drawing.Size(97, 17);
            this.chb_mode_onduleur.TabIndex = 33;
            this.chb_mode_onduleur.Text = "Mode onduleur";
            this.chb_mode_onduleur.UseVisualStyleBackColor = true;
            // 
            // bt_com
            // 
            this.bt_com.Enabled = false;
            this.bt_com.Location = new System.Drawing.Point(945, 71);
            this.bt_com.Name = "bt_com";
            this.bt_com.Size = new System.Drawing.Size(149, 39);
            this.bt_com.TabIndex = 120;
            this.bt_com.Text = "Commande en superposition";
            this.bt_com.UseVisualStyleBackColor = true;
            // 
            // chb_mode_multiniv
            // 
            this.chb_mode_multiniv.AutoSize = true;
            this.chb_mode_multiniv.Location = new System.Drawing.Point(13, 62);
            this.chb_mode_multiniv.Name = "chb_mode_multiniv";
            this.chb_mode_multiniv.Size = new System.Drawing.Size(118, 17);
            this.chb_mode_multiniv.TabIndex = 33;
            this.chb_mode_multiniv.Text = "Mode superposition";
            this.chb_mode_multiniv.UseVisualStyleBackColor = true;
            // 
            // gb_mode_fonctionnement
            // 
            this.gb_mode_fonctionnement.Controls.Add(this.cmb_mode_puissance);
            this.gb_mode_fonctionnement.Controls.Add(this.chb_surmodulation);
            this.gb_mode_fonctionnement.Controls.Add(this.chb_mode_multiniv);
            this.gb_mode_fonctionnement.Controls.Add(this.chb_mode_onduleur);
            this.gb_mode_fonctionnement.Location = new System.Drawing.Point(782, 25);
            this.gb_mode_fonctionnement.Name = "gb_mode_fonctionnement";
            this.gb_mode_fonctionnement.Size = new System.Drawing.Size(147, 109);
            this.gb_mode_fonctionnement.TabIndex = 1204;
            this.gb_mode_fonctionnement.TabStop = false;
            this.gb_mode_fonctionnement.Text = "mode de fonctionnement";
            // 
            // chb_surmodulation
            // 
            this.chb_surmodulation.AutoSize = true;
            this.chb_surmodulation.Enabled = false;
            this.chb_surmodulation.Location = new System.Drawing.Point(13, 80);
            this.chb_surmodulation.Name = "chb_surmodulation";
            this.chb_surmodulation.Size = new System.Drawing.Size(121, 17);
            this.chb_surmodulation.TabIndex = 33;
            this.chb_surmodulation.Text = "Mode surmodulation";
            this.chb_surmodulation.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(554, 433);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(203, 17);
            this.checkBox1.TabIndex = 29;
            this.checkBox1.Text = "Utiliser roue codeuse pour temps mort";
            this.checkBox1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.Click += new System.EventHandler(this.checkBox1_Click);
            // 
            // button3
            // 
            this.button3.Image = global::GeCoSwell.Properties.Resources.icons8_connected_16;
            this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.Location = new System.Drawing.Point(645, 457);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(85, 44);
            this.button3.TabIndex = 30;
            this.button3.Text = "Connection";
            this.button3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // panel_modulaire
            // 
            this.panel_modulaire.AutoScroll = true;
            this.panel_modulaire.Controls.Add(this.button3);
            this.panel_modulaire.Controls.Add(this.checkBox1);
            this.panel_modulaire.Location = new System.Drawing.Point(0, 74);
            this.panel_modulaire.Name = "panel_modulaire";
            this.panel_modulaire.Size = new System.Drawing.Size(776, 656);
            this.panel_modulaire.TabIndex = 1206;
            // 
            // Fenetre1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1362, 742);
            this.Controls.Add(this.panel_modulaire);
            this.Controls.Add(this.gb_mode_fonctionnement);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.b_lancer_serveur);
            this.Controls.Add(this.l_info_connection);
            this.Controls.Add(this.bt_com);
            this.Controls.Add(this.b_com_multi_niv);
            this.Controls.Add(this.b_Reception);
            this.Controls.Add(this.b_Envoi);
            this.Controls.Add(this.b_Connection);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Fenetre1";
            this.Text = "GeCoSwell (Generic Control of Switching cell)";
            this.Load += new System.EventHandler(this.Load_fenetre);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.gb_mode_fonctionnement.ResumeLayout(false);
            this.gb_mode_fonctionnement.PerformLayout();
            this.panel_modulaire.ResumeLayout(false);
            this.panel_modulaire.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void Val_manuel_Validating(object sender, CancelEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private System.Windows.Forms.Button b_Connection;
        private System.Windows.Forms.Button b_Envoi;
        private System.Windows.Forms.ToolStripMenuItem fichierToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sauvegarderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ouvrirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sauvegarderToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem quitterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem débugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem communicationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem déconnectéToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem envoiDesParamètresToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem réceptionDesParamètresToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem àProposToolStripMenuItem;
        private System.Windows.Forms.Button b_Reception;
        private System.Windows.Forms.Button b_com_multi_niv;
        private System.Windows.Forms.Label l_info_connection;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem relancerLeServeurToolStripMenuItem;
        private System.Windows.Forms.Button b_lancer_serveur;
        private System.Windows.Forms.ToolStripMenuItem outilsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem générationTableSinusToolStripMenuItem;
        private System.Windows.Forms.ComboBox cmb_mode_puissance;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem tsm_ouvrir;
        private System.Windows.Forms.ToolStripMenuItem tsm_sauvegarde;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem tsm_quit;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem tsm_connect;
        private System.Windows.Forms.ToolStripMenuItem tsm_deconnecte;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem tsm_send;
        private System.Windows.Forms.ToolStripMenuItem tsm_reception;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem tsm_serveur;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem13;
        private System.Windows.Forms.ToolStripMenuItem tsm_option;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem tsm_gen_sinus;
        private System.Windows.Forms.ToolStripMenuItem tsm_debug;
        private System.Windows.Forms.ToolStripMenuItem tsm_log;
        private System.Windows.Forms.ToolStripMenuItem superUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem18;
        private System.Windows.Forms.ToolStripMenuItem tsm_apropos;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.CheckBox chb_mode_onduleur;
        private System.Windows.Forms.Button bt_com;
        private System.Windows.Forms.CheckBox chb_mode_multiniv;
        private System.Windows.Forms.GroupBox gb_mode_fonctionnement;
        private System.Windows.Forms.CheckBox chb_surmodulation;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Panel panel_modulaire;
    }
}


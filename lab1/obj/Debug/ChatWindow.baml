   M S B A M L   `   `   `  ����  lab1    lab1.ChatWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -   5          � Title$1  +Запускай гуся работяги!��5   	   $	��600��6)   $	��400��66   $	��500��6B   $	��300��6R   $	��400��6a   $	��600��6p   .��5      � $	��5pxq�6    � RowDefinitions 5	   
   �� 5
      $	��20* �6   �� 5      $��3* �6   �� 5      $��1* �6   �� 5      $��1* �6   .+�5   
   �� -   5        $��ChatListBox��5      $��0��6&   $��Visible=�5      $��Disabled=�5      ��5   3   �� *
Messages+��5      �� 5      .R�5      �  5      �� 5      $��* �6,   �� 5      $��* �6,    � ColumnDefinitions 5      �� 5      $
��Auto �6/   �� 5      $
��Auto �6/   .+�5!      ��  �� TextAlignment$ Center=�6$   $��0��6;   $��0��6H   $
��Left=�6X   ��6n   �� *	UserLogin+�� 5"      $ Center=�6$   $��0��6;   $��1��6H   $��Right=�6X   ��6o   �� *
SendTime+�� 5#      $��WrapWithOverflow=�6$   $��1��6E   $��0��6R   $��2��6b   $
��Auto��6v   $��Stretch=�6�   ��6�   �� *MessageText+6�   5%      5&      5(      �� 5)   
   -   5         ��Name$ MessageInputTextBox��5)      $��1��6/    �� TextWrapping$
 Wrap=�6<   $��0,0,0,5q�6P   $3��-Введите ваше сообщение :)��6a   6�   �� 5*   
   -   5         ��Name$ SendMessageButton��5*      $��2��6,   $��Отправить��69   $��Bottom=�6m    �� 	IsDefault . 6�   �� 5+   
   -   5        $ ClearMessages��5+      $��3��6(   $��Отчистить чат��65   6M   5-      5.      
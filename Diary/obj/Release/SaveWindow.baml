   M S B A M L   `   `   `  ���� @  <Diary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:DiaryDiary      Diary.SaveWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     %localclr-namespace:Diary       � Title$  
SaveWindow��$	��600��$	��310�� � WindowStartupLocation$ CenterScreen=� � 
ResizeMode$ NoResize=����	#FFFAFAFA.��� $��10q����	#FFFAFAFA � ColumnDefinitions �� $��5* ��� $��2* � � RowDefinitions �� $��1* ��� $
��0.7* ��� $��6* ��� $��1* ��� $��1* �.+��� -    ��Name$ fileNameTextBox��$��2��$��0��$��10q�$
��Bold"�$��16(�.��	MyDiary� $��2��$��1��$��
10 0 10 10q� �� $��1* ��� $��1* �.+�	� $��0��$
��Bold"�$��16(� d� 	IsChecked$
 TrueY�.��.doc	� -    	�Name$ jsonRButton��$��1��$
��Bold"�$��16(�.��.json&   System.Windows.Controls.Calendar  -     Name$ saveDataCalendar��$��2��$��2��	   SelectionMode$	 SingleRange=��� -   $ pathTextBox��$��0��$��3��$��15q��� -   
 ��Name$
 
pathButton��$��1��$��3��.��Path�� -   $
 saveFileButton��$��2��$��4��.��Save
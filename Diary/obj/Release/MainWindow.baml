   M S B A M L   `   `   `  ���� R  NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R8clr-namespace:System.ComponentModel;assembly=WindowsBaseSystem.ComponentModel  @ <Diary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:DiaryDiary    Diary.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml      @d2http://schemas.microsoft.com/expression/blend/2008      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006      _ [MaterialDesignThemes.Wpf, Version=4.5.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  Bscm8clr-namespace:System.ComponentModel;assembly=WindowsBase  X TXceed.Wpf.Toolkit, Version=4.3.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  localclr-namespace:Diary  -     � Title$  Diary��$	��447��$	��750��$	��447��$	��750�� � WindowStartupLocation$ CenterScreen=�.��� -    �Name$
 grid�� � 	Resources    TimePickerTemplate)  #MaterialDesignThemes.Wpf.TimePicker   	Is24Hours   SelectedTime �� Path �� UpdateSourceTrigger��%O   &	        �� .R�   .  �� $ ExecuteTime�$ PropertyChanged=�����  �� EndPoint$ 0.5,1&�	 �� 
StartPoint$	 0.5,0&�.��� 
 � Color$
 	#FFFAFAFA�� � Offset$ 0Q�� $
 	#FF673AB7��$ 1Q�.+�� $��Center=� � ColumnDefinitions �� $��* ��� -    ��Name$ mainGrid��$��0 �.+��� -    ��Name$ panel�� �� CornerRadius$ 10�����	#FFF1F1F1$��25 20 25 30q�$	��710��$��0��._���  �� Orientation$ Vertical=�$��0 10 0 0q�.+��  �� $
��Auto ��� .+��� -    ��Name$ 
dateButton�����	#FF673AB7$
��Left=�
�������$��0q�$��20 0 0 0q�$��0����Z� .��]� .+��� -    ��Name$ 	buttonDay��$	��Top=�$��Center=��� -   $ buttonMonth��$��Bottom=�$��Center=�$��0 0 0 5q�� $��1��$��
10 10 20 0q�$	��100�� � RowDefinitions �� $��2* ��� $��1* ��� $��2* � �� �� �� .+�� -    �Name$ taskProgress�� � Minimum$ 0Q� � Maximum$	 100Q� � Value$ 50Q�$��10�� � Orientation$ 
Horizontal=�$��1��$��2��$��0 0 0 10q��� -	   $ clearButton��$��0��$��2��$��0q�$	��200��
����������	#FF673AB7��Z� .��Clear�� -
   $ deleteButton��$��1��$��2��$��10 0 0 0q�$	��200��
����������	#FF673AB7��Z� .��Delete�� -   $ 
saveButton��$��2��$��1��$��	10 0 0 10q�
����������	#FF673AB7��Z� .��Save�� -   $ 	addButton��$��2��$��2��$��10 0 0 0q�
����������	#FF673AB7��Z� .��Add� $��
20 20 20 0q� �� $��50 ��� $	��100 ��� $��* ��� $��75 �.+��� -   $ importentSortButton��$��0����Z� .��★�� -   $ timeSortButton��$��1����Z� .��Time�� -   $ nameSortButton��$��2����Z� .��Name�� -   $ doneSortButton��$��3����Z� .��Done&   System.Windows.Controls.DataGrid  -     Name$ dgDiary��$��16(�$
��Bold"�   AutoGenerateColumns .  $   HorizontalScrollBarVisibility$ Disabled=�$��
20 0 20 10q�"   VerticalScrollBarVisibility$ Hidden=�   CanUserSortColumns .    HeadersVisibility$
 None=�   Columns 4  .System.Windows.Controls.DataGridCheckBoxColumn  ,  &System.Windows.Controls.DataGridColumn    
IsReadOnly  .  !   Width5  /System.Windows.Controls.DataGridLengthConverter$! 50 1  +System.Windows.Controls.DataGridBoundColumn"   Binding" �� $ IsImportent�$ PropertyChanged=�4  .System.Windows.Controls.DataGridTemplateColumn  #   CellTemplate## [  $	! 100 1  +MaterialDesignThemes.Wpf.DataGridTextColumn    .  $! * " �� $
 Name�    .  $! 75 " �� $ IsDone�$ PropertyChanged=�$   RowDetailsTemplate$ �� .R��  �� $��* ��� $��50 �.+��� % �� TextAlignment$% Center=�$��0������ $
 Text�$ PropertyChanged=��� $ closeTooltip��-   $��1��$��^��&	   System.Windows.Controls.Calendar	  -   & 	 Name$& dateCalendar��$	��250��$	��360��$��Center=�$��0 0 25 0q�$y�	Collapsed=�$��1��
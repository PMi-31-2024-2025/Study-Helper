   M S B A M L   `   `   `  ����   StudyHelperApp%    StudyHelperApp.Views.NoteWindow   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  Notes��$	��550��$	��700�� � WindowStartupLocation$ CenterScreen=����#F9F9F9.��]� .+�]� $	��Top=����#4B0082$��70��.+��� $��Study Helper��
�������$��28(�$
��Bold"�$��Center=�$��15,0,0,0q�$
��Left=���  �� Orientation$ 
Horizontal=�$��Right=�$��Center=�$��0,0,10,0q�.+��� -   $
��Home��$��77��$��27���� -   $��
To-Do List��$��84���� -   $��Grades��$��81���� -   $��Login��$��79���� -   $��Logout��$��77��� $��10q� � ColumnDefinitions �� $	��250 ��� $��* �.+��� $��0��
����������#D3D3D3$��1q� �� CornerRadius$ 5�� �� Padding$ 5q�._��� .+��� $��
Your Notes��$��18(�$
��Bold"����#4B0082$��0,0,0,10q��� -   $��NotesListView�����#D3D3D3���#F5F5F5 �� View �� .P��� $��
Created At�� �� Width$	 200�� �� DisplayMemberBinding �� *	CreatedAt+�� $��1��$��10q�.+��� $��Note Content��$��18(�$
��Bold"����#4B0082$��0,0,0,10q��� -   $��NoteTextBox��	 �� AcceptsReturn$
	 True��"
 �� VerticalScrollBarVisibility$

 Auto=�$	��300�����#D3D3D3���#F5F5F5$��5q�$��14(��� $ 
Horizontal=�$��Right=�$��0,10,0,0q�.+��� -   $
��Save�����#4B0082
�������$
��Bold"�$	��100��$��5q� � Cursor$
 Hand���� -	   $��Delete�����#DC143C
�������$
��Bold"�$	��100��$��5q�$
 Hand��
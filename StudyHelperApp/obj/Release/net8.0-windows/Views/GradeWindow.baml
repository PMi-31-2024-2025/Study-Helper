   M S B A M L   `   `   `  ����   StudyHelperApp&     StudyHelperApp.Views.GradeWindow   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  Grades��$	��750��$	��900�� � WindowStartupLocation$ CenterScreen=����#F9F9F9.��]� $��	0,0,0,-16q�.+�]� $	��Top=����#4B0082$��70��.+��� $��Study Helper��
�������$��28(�$
��Bold"�$��Center=�$��15,0,0,0q�$
��Left=���  �� Orientation$ 
Horizontal=�$��Right=�$��Center=�$��0,0,10,0q�.+��� -   $
��Home��$	��106���� -   $��Notes��$	��101���� -   $��
To-Do List��$��95���� -   $��Login��$��91���� -   $��Logout��$��97��� $��20q� � RowDefinitions �� $
��Auto ��� $��2* ��� $��3* �.+��� $��Your Grades Overview��$��24(�$
��Bold"����#4B0082$��	10,0,0,10q�$	��Top=��� $��1�� �� CornerRadius$ 10��
����������#4B0082$��2q� �� Padding$ 10q�._��� -   $��GradesListView�� �� View �� .P��� $��Subject�� �� Width$	 200�� �� DisplayMemberBinding �� *SubjectName+�� $��Grade��$	 100�� �� *Value+�� $
��Date��$	 200�� �� *	CreatedAt+�� $��2��$ 10��
����������#4B0082$��2q�$ 10q�$��0,10,0,0q�._��� .+��� $��Average Grades by Subject��$��20(�$
��Bold"����#4B0082$��0,0,0,10q��� $	��100��$
��Auto=�.���� -   $��AverageGradesListView�� �� .P��� $��Subject��$	 200�� �� *SubjectName+�� $��Average Grade��$	 150�� �� *AverageGrade+�� $��25q�.+��� -   	 ��Name$	 BarChartCanvas��$	��116�����#E6E6FA
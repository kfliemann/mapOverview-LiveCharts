   M S B A M L   `   `   `  ����F  BmapOverview, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:mapOverviewmapOverview  5   	   U QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf 5   	       mapOverview.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:mapOverview     5   	   Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     5   	     � Title$  CoronaDisplayer��5   	   $
��1020��6!   $
��1080��6/   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��auto �6   �� 5      .+�5   
   � $��0��6   
�������.+�5      �� $��Center=�6   $��20(�61   .��6?   86COVID-19-Fälle der letzten 7 Tage / 100.000 Einwohner6v   5      � 5   
   -   5         �Name$ 
gridGeoMap��5      $��1��6#   .+�5      � -   5        $��MyPopup��5      $��
MousePoint=�6#    � HorizontalOffset$ -1��6:    � VerticalOffset$ -1��6P   .��5      �� 
����ح�.+�5      �� -   5         ��Name$ districtNamePop��5       $��Center=�69   6X   �� 5      -   5        $ districtInfo��5       68   �� 5      -   5        $��10q�5   0   .��6<   
Schließen6G   5      5      5      5      5      
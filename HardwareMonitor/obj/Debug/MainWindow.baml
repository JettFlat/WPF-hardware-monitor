   M S B A M L   `   `   `  ����J  FHardwareMonitor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:HardwareMonitorHardwareMonitor  5   	   c _System.Windows.Interactivity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity 5   	        HardwareMonitor.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:HardwareMonitor     5   	   ^iPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity     5   	   V RMaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	   	      MaterialDesignBody#���   5
   	   $��Regular"�5   	   $��13(�5   	   &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5   	      TextRenderingMode$
 Auto=�5   	     VS2012WindowStyle#��[ 5   	     MaterialDesignPaper#���  5   	     MaterialDesignFont#���  5   	    � SizeToContent$ WidthAndHeight=�6;   $	��160��6Z   $	��160��6j    � DataContext 5         HardwareMonitor.VM  5   
    � 	Resources 5       �� 
TargetType �� Property �� Value��%�   '��       '�� J     �� 5   
   " ��6   .�5      ��  � ��
 �   �6+   5      �� 5   
   " ��6   .�5      ��  � ��
 �   �6+   5      5      .��5"      � .+�5#   
   �� .+�5$      �� ��6   �� *SysData.Hards+��5%      �� 5&      .R�5'      �� .+�5(      �� ��6(   ��  �� Path$ Hardware.Name��� 5)      ��6&   �� *GraphicValues+��5*   "   �� 5+   &   .R�5,   *   � 5-   '   ��5/   "   �� 50   &   .R�51   *   �� .+�52   .   ]� .+�53   2   �� $
��Left=�6<   ��6R   �� $ HardName��� 54   2   $��Right=�69   $��Right=�6P     MaterialDesignFlatButton#��[ 6�   ��6l   �� 	 �� RelativeSource	 �� 
 �� AncestorType$
 WindowL� �� Mode$ FindAncestor=�$ DataContext.Delete���6�   �� .��55   6   '  !MaterialDesignThemes.Wpf.PackIcon  �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=null]]   Kind$ DeleteCircle=�6N   
�������6b   56   3   57   /   �� 58   .   $	��150��69    �� Orientation$ 
Horizontal=�6H   .+�59   2   �� $��30(�6=   $��Impact)�6K   
���������6r   �� $ CurrentValue��� 5:   2   $�� ��6=   �� 5;   2   $��Bottom=�6W   ��6=   �� *
UnitType+6W   5<   /   �� 5=   .   $ 
Horizontal=�69   .+�5>   2   � $��60��6;   $	��110��6J   
��� E�� �� StrokeEndLineCap$ Round=�6l   $��2��6�    � Points 6�   �� $ Points��� 5?   2   .+�5@   6   �� $	��Top=�6Z   ��6@   �� $	 Max�6Z   �� 5A   6   $��Bottom=�6Z   ��6@   �� $	 Min�6Z   5B   3   5C   /   5D   +   5E   '   5G      5H      5I      5K      �� 5L      
�������#��[ 6F   ��6   �� *	ToDefault+.��5M      �� $ 
Horizontal=�6   .+�5N      �� $��Reset��6      5O      $ Settings=�6.   
�������6>   5P      5Q      5R      5S      5T      
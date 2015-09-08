//
// This code based on code available here:
//
//  http://www.codeproject.com/KB/WPF/WPFJoshSmith.aspx
//

namespace Client.Adorner
{
	using System;
	using System.Collections;
	using System.Windows;
	using System.Windows.Media;

	//
	// This class is an adorner that allows a FrameworkElement derived class to adorn another FrameworkElement.
	//
	public class FrameworkElementAdorner : System.Windows.Documents.Adorner
	{
	//
	// The framework element that is the adorner. 
	//
	private readonly FrameworkElement child;

	//
	// Placement of the child.
	//
	private readonly AdornerPlacement _horizontalAdornerPlacement = AdornerPlacement.Inside;
	private readonly AdornerPlacement _verticalAdornerPlacement = AdornerPlacement.Inside;

	//
	// Offset of the child.
	//
	private readonly double _offsetX;
	private readonly double _offsetY;

	//
	// Position of the child (when not set to NaN).
	//

	public FrameworkElementAdorner(FrameworkElement adornerChildElement, UIElement adornedElement) : base(adornedElement)
	{
		PositionX = Double.NaN;
		PositionY = Double.NaN;
		child = adornerChildElement;
		
		AddLogicalChild(adornerChildElement);
		AddVisualChild(adornerChildElement);
	}

	public FrameworkElementAdorner(FrameworkElement adornerChildElement, FrameworkElement adornedElement, AdornerPlacement horizontalAdornerPlacement, 
		AdornerPlacement verticalAdornerPlacement, double offsetX, double offsetY) : base(adornedElement)
	{
		PositionX = Double.NaN;
		PositionY = Double.NaN;
		child = adornerChildElement;
		_horizontalAdornerPlacement = horizontalAdornerPlacement;
		_verticalAdornerPlacement = verticalAdornerPlacement;
		_offsetX = offsetX;
		_offsetY = offsetY;

		adornedElement.SizeChanged += AdornedElementSizeChanged;
		AddLogicalChild(adornerChildElement);
		AddVisualChild(adornerChildElement);
	}

	/// <summary>
	/// Event raised when the adorned control's size has changed.
	/// </summary>
	private void AdornedElementSizeChanged(object sender, SizeChangedEventArgs e)
	{
		InvalidateMeasure();
	}

	//
	// Position of the child (when not set to NaN).
	//
	public double PositionX { get; set; }

	public double PositionY { get; set; }

	protected override Size MeasureOverride(Size constraint)
	{
		child.Measure(constraint);
		return child.DesiredSize;
	}

	/// <summary>
	/// Determine the X coordinate of the child.
	/// </summary>
	private double DetermineX()
	{
		switch (child.HorizontalAlignment)
		{
			case HorizontalAlignment.Left:
			{
				return _horizontalAdornerPlacement == AdornerPlacement.Outside
						? -child.DesiredSize.Width + _offsetX
						: _offsetX;
			}
			case HorizontalAlignment.Right:
			{
				return _horizontalAdornerPlacement == AdornerPlacement.Outside
						? AdornedElement.ActualWidth + _offsetX
						: AdornedElement.ActualWidth - child.DesiredSize.Width + _offsetX;
			}
			case HorizontalAlignment.Center:
			{
				return AdornedElement.ActualWidth / 2 - child.DesiredSize.Width / 2 + _offsetX;
			}
			case HorizontalAlignment.Stretch:
			{
				return 0.0;
			}
		}

		return 0.0;
	}

	/// <summary>
	/// Determine the Y coordinate of the child.
	/// </summary>
	private double DetermineY()
	{
		switch (child.VerticalAlignment)
		{
			case VerticalAlignment.Top:
			{
				return _verticalAdornerPlacement == AdornerPlacement.Outside
						? -child.DesiredSize.Height + _offsetY
						: _offsetY;
			}
			case VerticalAlignment.Bottom:
			{
				return _verticalAdornerPlacement == AdornerPlacement.Outside
						? AdornedElement.ActualHeight + _offsetY
						: AdornedElement.ActualHeight - child.DesiredSize.Height + _offsetY;
			}
			case VerticalAlignment.Center:
			{
				return AdornedElement.ActualHeight / 2 - child.DesiredSize.Height / 2 + _offsetY;
			}
			case VerticalAlignment.Stretch:
			{
				return 0.0;
			}
		}

		return 0.0;
	}

	/// <summary>
	/// Determine the width of the child.
	/// </summary>
	private double DetermineWidth()
	{
		if (!Double.IsNaN(PositionX))
		{
			return child.DesiredSize.Width;
		}

		switch (child.HorizontalAlignment)
		{
			case HorizontalAlignment.Left:
			{
				return child.DesiredSize.Width;
			}
			case HorizontalAlignment.Right:
			{
				return child.DesiredSize.Width;
			}
			case HorizontalAlignment.Center:
			{
				return child.DesiredSize.Width;
			}
			case HorizontalAlignment.Stretch:
			{
				return AdornedElement.ActualWidth;
			}
		}

		return 0.0;
	}

	/// <summary>
	/// Determine the height of the child.
	/// </summary>
	private double DetermineHeight()
	{
		if (!Double.IsNaN(PositionY))
		{
			return child.DesiredSize.Height;
		}

		switch (child.VerticalAlignment)
		{
			case VerticalAlignment.Top:
			{
				return child.DesiredSize.Height;
			}
			case VerticalAlignment.Bottom:
			{
				return child.DesiredSize.Height;
			}
			case VerticalAlignment.Center:
			{
				return child.DesiredSize.Height; 
			}
			case VerticalAlignment.Stretch:
			{
				return AdornedElement.ActualHeight;
			}
		}

		return 0.0;
	}

	protected override Size ArrangeOverride(Size finalSize)
	{
		var x = PositionX;
		if (Double.IsNaN(x))
		{
			x = DetermineX();
		}
		var y = PositionY;
		if (Double.IsNaN(y))
		{
			y = DetermineY();
		}

		child.Arrange(new Rect(x, y, DetermineWidth(), DetermineHeight()));
		return finalSize;
	}

	protected override Int32 VisualChildrenCount
	{
		get { return 1; }
	}

	protected override Visual GetVisualChild(Int32 index)
	{
		return child;
	}

	protected override IEnumerator LogicalChildren
	{
		get
		{
			var list = new ArrayList {child};
			return list.GetEnumerator();
		}
	}

	/// <summary>
	/// Disconnect the child element from the visual tree so that it may be reused later.
	/// </summary>
	public void DisconnectChild()
	{
		RemoveLogicalChild(child);
		RemoveVisualChild(child);
	}

	/// <summary>
	/// Override AdornedElement from base class for less type-checking.
	/// </summary>
	public new FrameworkElement AdornedElement
	{
		get
		{
			return (FrameworkElement)base.AdornedElement;
		}
	}
    }
}
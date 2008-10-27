/*
 * ObjectListView - A listview to show various aspects of a collection of objects
 *
 * Author: Phillip Piper
 * Date: 9/10/2006 11:15 AM
 *
 * Change log:
 * 2008-10-28  JPP  - SelectedObjects is now an IList, rather than an ArrayList. This allows
 *                    it to accept generic list (eg List<File>).
 * 2008-10-09  JPP  - Support indeterminate checkbox values. 
 *                    CheckStateGetter now returns a bool? rather than a bool. 
 *                    CheckStatePutter does not return anything (BREAKING CHANGE).
 * 2008-10-08  JPP  - Added setFocus parameter to SelectObject(), which allows focus to be set
 *                    at the same time as selecting.
 * 2008-09-27  JPP  - BIG CHANGE: Fissioned this file into separate files for each component
 * 2008-09-24  JPP  - Corrected bug with owner drawn lists where a column 0 with a renderer
 *                    would draw at column 0 even if column 0 was dragged to another position.
 *                  - Correctly handle space filling columns when columns are added/removed
 * 2008-09-16  JPP  - Consistently use try..finally for BeginUpdate()/EndUpdate() pairs
 * 2008-08-24  JPP  - If LastSortOrder is None when adding objects, don't force a resort.
 * 2008-08-22  JPP  - Catch and ignore some problems with setting TopIndex on FastObjectListViews.
 * 2008-08-05  JPP  - In the right-click column select menu, columns are now sorted by display order, rather than alphabetically
 * v1.13
 * 2008-07-23  JPP  - Consistently use copy-on-write semantics with Add/RemoveObject methods
 * 2008-07-10  JPP  - Enable validation on cell editors through a CellEditValidating event.
 *                    (thanks to Artiom Chilaru for the initial suggestion and implementation).
 * 2008-07-09  JPP  - Added HeaderControl.Handle to allow OLV to be used within UserControls.
 *                    (thanks to Michael Coffey for tracking this down).
 * 2008-06-23  JPP  - Split the more generally useful CopyObjectsToClipboard() method
 *                    out of CopySelectionToClipboard()
 * 2008-06-22  JPP  - Added AlwaysGroupByColumn and AlwaysGroupBySortOrder, which
 *                    force the list view to always be grouped by a particular column.
 * 2008-05-31  JPP  - Allow check boxes on FastObjectListViews
 *                  - Added CheckedObject and CheckedObjects properties
 * 2008-05-11  JPP  - Allow selection foreground and background colors to be changed.
 *                    Windows doesn't allow this, so we can only make it happen when owner
 *                    drawing. Set the HighlightForegroundColor and  HighlightBackgroundColor
 *                    properties and then call EnableCustomSelectionColors().
 * v1.12
 * 2008-05-08  JPP  - Fixed bug where the column select menu would not appear if the
 *                    ObjectListView has a context menu installed.
 * 2008-05-05  JPP  - Non detail views can now be owner drawn. The renderer installed for
 *                    primary column is given the chance to render the whole item.
 *                    See BusinessCardRenderer in the demo for an example.
 *                  - BREAKING CHANGE: RenderDelegate now returns a bool to indicate if default
 *                    rendering should be done. Previously returned void. Only important if your
 *                    code used RendererDelegate directly. Renderers derived from BaseRenderer
 *                    are unchanged.
 * 2008-05-03  JPP  - Changed cell editing to use values directly when the values are Strings.
 *                    Previously, values were always handed to the AspectToStringConverter.
 *                  - When editing a cell, tabbing no longer tries to edit the next subitem
 *                    when not in details view!
 * 2008-05-02  JPP  - MappedImageRenderer can now handle a Aspects that return a collection
 *                    of values. Each value will be drawn as its own image.
 *                  - Made AddObjects() and RemoveObjects() work for all flavours (or at least not crash)
 *                  - Fixed bug with clearing virtual lists that has been scrolled vertically
 *                  - Made TopItemIndex work with virtual lists.
 * 2008-05-01  JPP  - Added AddObjects() and RemoveObjects() to allow faster mods to the list
 *                  - Reorganised public properties. Now alphabetical.
 *                  - Made the class ObjectListViewState internal, as it always should have been.
 * v1.11
 * 2008-04-29  JPP  - Preserve scroll position when building the list or changing columns.
 *                  - Added TopItemIndex property. Due to problems with the underlying control, this
 *                    property is not always reliable. See property docs for info.
 * 2008-04-27  JPP  - Added SelectedIndex property.
 *                  - Use a different, more general strategy to handle Invoke(). Removed all delegates
 *                    that were only declared to support Invoke().
 *                  - Check all native structures for 64-bit correctness.
 * 2008-04-25  JPP  - Released on SourceForge.
 * 2008-04-13  JPP  - Added ColumnRightClick event.
 *                  - Made the assembly CLS-compliant. To do this, our cell editors were made internal, and
 *                    the constraint on FlagRenderer template parameter was removed (the type must still
 *                    be an IConvertible, but if it isn't, the error will be caught at runtime, not compile time).
 * 2008-04-12  JPP  - Changed HandleHeaderRightClick() to have a columnIndex parameter, which tells
 *                    exactly which column was right-clicked.
 * 2008-03-31  JPP  - Added SaveState() and RestoreState()
 *                  - When cell editing, scrolling with a mouse wheel now ends the edit operation.
 * v1.10
 * 2008-03-25  JPP  - Added space filling columns. See OLVColumn.FreeSpaceProportion property for details.
 *                    A space filling columns fills all (or a portion) of the width unoccupied by other columns.
 * 2008-03-23  JPP  - Finished tinkering with support for Mono. Compile with conditional compilation symbol 'MONO'
 *                    to enable. On Windows, current problems with Mono:
 *                    - grid lines on virtual lists crashes
 *                    - when grouped, items sometimes are not drawn when any item is scrolled out of view
 *                    - i can't seem to get owner drawing to work
 *                    - when editing cell values, the editing controls always appear behind the listview,
 *                      where they function fine -- the user just can't see them :-)
 * 2008-03-16  JPP  - Added some methods suggested by Chris Marlowe (thanks for the suggestions Chris)
 *                    - ClearObjects()
 *                    - GetCheckedObject(), GetCheckedObjects()
 *                    - GetItemAt() variation that gets both the item and the column under a point
 * 2008-02-28  JPP  - Fixed bug with subitem colors when using OwnerDrawn lists and a RowFormatter.
 * v1.9.1
 * 2008-01-29  JPP  - Fixed bug that caused owner-drawn virtual lists to use 100% CPU
 *                  - Added FlagRenderer to help draw bitwise-OR'ed flag values
 * 2008-01-23  JPP  - Fixed bug (introduced in v1.9) that made alternate row colour with groups not quite right
 *                  - Ensure that DesignerSerializationVisibility.Hidden is set on all non-browsable properties
 *                  - Make sure that sort indicators are shown after changing which columns are visible
 * 2008-01-21  JPP  - Added FastObjectListView
 * v1.9
 * 2008-01-18  JPP  - Added IncrementalUpdate()
 * 2008-01-16  JPP  - Right clicking on column header will allow the user to choose which columns are visible.
 *                    Set SelectColumnsOnRightClick to false to prevent this behaviour.
 *                  - Added ImagesRenderer to draw more than one images in a column
 *                  - Changed the positioning of the empty list msg to use all the client area. Thanks to Matze.
 * 2007-12-13  JPP  - Added CopySelectionToClipboard(). Ctrl-C invokes this method. Supports text
 *                    and HTML formats.
 * 2007-12-12  JPP  - Added support for checkboxes via CheckStateGetter and CheckStatePutter properties.
 *                  - Made ObjectListView and OLVColumn into partial classes so that others can extend them.
 * 2007-12-09  JPP  - Added ability to have hidden columns, i.e. columns that the ObjectListView knows
 *                    about but that are not visible to the user. Controlled by OLVColumn.IsVisible.
 *                    Added ColumnSelectionForm to the project to show how it could be used in an application.
 *
 * v1.8
 * 2007-11-26  JPP  - Cell editing fully functional
 * 2007-11-21  JPP  - Added SelectionChanged event. This event is triggered once when the
 *                    selection changes, no matter how many items are selected or deselected (in
 *                    contrast to SelectedIndexChanged which is called once for every row that
 *                    is selected or deselected). Thanks to lupokehl42 (Daniel) for his suggestions and
 *                    improvements on this idea.
 * 2007-11-19  JPP  - First take at cell editing
 * 2007-11-17  JPP  - Changed so that items within a group are not sorted if lastSortOrder == None
 *                  - Only call MakeSortIndicatorImages() if we haven't already made the sort indicators
 *                    (Corrected misspelling in the name of the method too)
 * 2007-11-06  JPP  - Added ability to have secondary sort criteria when sorting
 *                    (SecondarySortColumn and SecondarySortOrder properties)
 *                  - Added SortGroupItemsByPrimaryColumn to allow group items to be sorted by the
 *                    primary column. Previous default was to sort by the grouping column.
 * v1.7
 * No big changes to this version but made to work with ListViewPrinter and released with it.
 *
 * 2007-11-05  JPP  - Changed BaseRenderer to use DrawString() rather than TextRenderer, since TextRenderer
 *                    does not work when printing.
 * v1.6
 * 2007-11-03  JPP  - Fixed some bugs in the rebuilding of DataListView.
 * 2007-10-31  JPP  - Changed to use builtin sort indicators on XP and later. This also avoids alignment
 *                    problems on Vista. (thanks to gravybod for the suggestion and example implementation)
 * 2007-10-21  JPP  - Added MinimumWidth and MaximumWidth properties to OLVColumn.
 *                  - Added ability for BuildList() to preserve selection. Calling BuildList() directly
 *                    tries to preserve selection; calling SetObjects() does not.
 *                  - Added SelectAll() and DeselectAll() methods. Useful for working with large lists.
 * 2007-10-08  JPP  - Added GetNextItem() and GetPreviousItem(), which walk sequentially through the
 *                    listview items, even when the view is grouped.
 *                  - Added SelectedItem property
 * 2007-09-28  JPP  - Optimized aspect-to-string conversion. BuildList() 15% faster.
 *                  - Added empty implementation of RefreshObjects() to VirtualObjectListView since
 *                    RefreshObjects() cannot work on virtual lists.
 * 2007-09-13  JPP  - Corrected bug with custom sorter in VirtualObjectListView (thanks for mpgjunky)
 * 2007-09-07  JPP  - Corrected image scaling bug in DrawAlignedImage() (thanks to krita970)
 * 2007-08-29  JPP  - Allow item count labels on groups to be set per column (thanks to cmarlow for idea)
 * 2007-08-14  JPP  - Major rework of DataListView based on Ian Griffiths's great work
 * 2007-08-11  JPP  - When empty, the control can now draw a "List Empty" message
 *                  - Added GetColumn() and GetItem() methods
 * v1.5
 * 2007-08-03  JPP  - Support animated GIFs in ImageRenderer
 *                  - Allow height of rows to be specified - EXPERIMENTAL!
 * 2007-07-26  JPP  - Optimised redrawing of owner-drawn lists by remembering the update rect
 *                  - Allow sort indicators to be turned off
 * 2007-06-30  JPP  - Added RowFormatter delegate
 *                  - Allow a different label when there is only one item in a group (thanks to cmarlow)
 * v1.4
 * 2007-04-12  JPP  - Allow owner drawn on steriods!
 *                  - Column headers now display sort indicators
 *                  - ImageGetter delegates can now return ints, strings or Images
 *                    (Images are only visible if the list is owner drawn)
 *                  - Added OLVColumn.MakeGroupies to help with group partitioning
 *                  - All normal listview views are now supported
 *                  - Allow dotted aspect names, e.g. Owner.Workgroup.Name (thanks to OlafD)
 *                  - Added SelectedObject and SelectedObjects properties
 * v1.3
 * 2007-03-01  JPP  - Added DataListView
 *                  - Added VirtualObjectListView
 * 					- Added Freeze/Unfreeze capabilities
 *                  - Allowed sort handler to be installed
 *                  - Simplified sort comparisons: handles 95% of cases with only 6 lines of code!
 *                  - Fixed bug with alternative line colors on unsorted lists (thanks to cmarlow)
 * 2007-01-13  JPP  - Fixed bug with lastSortOrder (thanks to Kwan Fu Sit)
 *                  - Non-OLVColumns are no longer allowed
 * 2007-01-04  JPP  - Clear sorter before rebuilding list. 10x faster! (thanks to aaberg)
 *                  - Include GetField in GetAspectByName() so field values can be Invoked too.
 * 					- Fixed subtle bug in RefreshItem() that erased background colors.
 * 2006-11-01  JPP  - Added alternate line colouring
 * 2006-10-20  JPP  - Refactored all sorting comparisons and made it extendable. See ComparerManager.
 *                  - Improved IDE integration
 *                  - Made control DoubleBuffered
 *                  - Added object selection methods
 * 2006-10-13  JPP  Implemented grouping and column sorting
 * 2006-10-09  JPP  Initial version
 *
 * TO DO:
 * - Extend MappedImageRender to be able to draw more than image if its Aspect returns an ICollection.
 *
 * Copyright (C) 2006-2008 Phillip Piper
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 *
 * If you wish to use this code in a closed source application, please contact phillip_piper@bigfoot.com.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace BrightIdeasSoftware
{
    /// <summary>
    /// An object list displays 'aspects' of a collection of objects in a listview control.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The intelligence for this control is in the columns. OLVColumns are
    /// extended so they understand how to fetch an 'aspect' from each row
    /// object. They also understand how to sort by their aspect, and
    /// how to group them.
    /// </para>
    /// <para>
    /// Aspects are extracted by giving the name of a method to be called or a
    /// property to be fetched. These names can be simple names or they can be dotted
    /// to chain property access e.g. "Owner.Address.Postcode".
    /// Aspects can also be extracted by installing a delegate.
    /// </para>
    /// <para>
    /// Sorting by column clicking and grouping by column are handled automatically.
    /// </para>
    /// <para>
    /// Right clicking on the column header should present a popup menu that allows the user to
    /// choose which columns will be visible in the list. This behaviour can be disabled by
    /// setting SelectColumnsOnRightClick to false.
    /// </para>
    /// <para>
    /// This list puts sort indicators in the column headers to show the column sorting direction.
    /// On Windows XP and later, the system standard images are used.
    /// If you wish to replace the standard images with your own images, put entries in the small image list
    /// with the key values "sort-indicator-up" and "sort-indicator-down".
    /// </para>
    /// <para>
    /// For these classes to build correctly, the project must have references to these assemblies:
    /// <list>
    /// <item>System</item>
    /// <item>System.Data</item>
    /// <item>System.Design</item>
    /// <item>System.Drawing</item>
    /// <item>System.Windows.Forms (obviously)</item>
    /// </list>
    /// </para>
    /// </remarks>
    public partial class ObjectListView : ListView, ISupportInitialize
    {
        /// <summary>
        /// Create an ObjectListView
        /// </summary>
        public ObjectListView()
            : base()
        {
            this.ColumnClick += new ColumnClickEventHandler(this.HandleColumnClick);
            this.ItemCheck += new ItemCheckEventHandler(this.HandleItemCheck);
            this.Layout += new LayoutEventHandler(this.HandleLayout);
            this.ColumnWidthChanging += new ColumnWidthChangingEventHandler(this.HandleColumnWidthChanging);
            this.ColumnWidthChanged += new ColumnWidthChangedEventHandler(this.HandleColumnWidthChanged);

            base.View = View.Details;
            this.DoubleBuffered = true; // kill nasty flickers. hiss... me hates 'em
            this.AlternateRowBackColor = Color.Empty;
            this.ShowSortIndicators = true;
            this.isOwnerOfObjects = false;
        }

        #region Public properties

        /// <summary>
        /// Get or set all the columns that this control knows about.
        /// Only those columns where IsVisible is true will be seen by the user.
        /// </summary>
        /// <remarks>If you want to add new columns programmatically, add them to
        /// AllColumns and then call RebuildColumns(). Normally, you do not have to
        /// deal with this property directly. Just use the IDE.</remarks>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<OLVColumn> AllColumns
        {
            get
            {
                // If someone has wiped out the columns, put the list back
                if (allColumns == null)
                    allColumns = new List<OLVColumn>();

                // If we don't know the columns, use the columns from the control.
                // This handles legacy cases
                if (allColumns.Count == 0 && this.Columns.Count > 0) {
                    for (int i = 0; i < this.Columns.Count; i++) {
                        this.allColumns.Add(this.GetColumn(i));
                    }
                }
                return allColumns;
            }
            set { allColumns = value; }
        }
        private List<OLVColumn> allColumns = new List<OLVColumn>();

        /// <summary>
        /// If every second row has a background different to the control, what color should it be?
        /// </summary>
        [Category("Appearance"),
         Description("If using alternate colors, what color should alterate rows be?"),
         DefaultValue(typeof(Color), "Empty")]
        public Color AlternateRowBackColor
        {
            get { return alternateRowBackColor; }
            set { alternateRowBackColor = value; }
        }
        private Color alternateRowBackColor = Color.Empty;

        /// <summary>
        /// Return the alternate row background color that has been set, or the default color
        /// </summary>
        [Browsable(false)]
        public Color AlternateRowBackColorOrDefault
        {
            get
            {
                if (alternateRowBackColor == Color.Empty)
                    return Color.LemonChiffon;
                else
                    return alternateRowBackColor;
            }
        }

        /// <summary>
        /// This property forces the ObjectListView to always group items by the given column.
        /// </summary>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OLVColumn AlwaysGroupByColumn
        {
            get { return alwaysGroupByColumn; }
            set { alwaysGroupByColumn = value; }
        }
        private OLVColumn alwaysGroupByColumn;

        /// <summary>
        /// If AlwaysGroupByColumn is not null, this property will be used to decide how
        /// those groups are sorted. If this property has the value SortOrder.None, then
        /// the sort order will toggle according to the users last header click.
        /// </summary>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SortOrder AlwaysGroupBySortOrder
        {
            get { return alwaysGroupBySortOrder; }
            set { alwaysGroupBySortOrder = value; }
        }
        private SortOrder alwaysGroupBySortOrder = SortOrder.None;

        /// <summary>
        /// Give access to the image list that is actually being used by the control
        /// </summary>
        [Browsable(false)]
        public ImageList BaseSmallImageList
        {
            get { return base.SmallImageList; }
        }

        /// <summary>
        /// How does a user indicate that they want to edit cells?
        /// </summary>
        public enum CellEditActivateMode
        {
            /// <summary>
            /// This list cannot be edited. F2 does nothing.
            /// </summary>
            None = 0,

            /// <summary>
            /// A single click on  a <strong>subitem</strong> will edit the value. Single clicking the primary column,
            /// selects the row just like normal. The user must press F2 to edit the primary column.
            /// </summary>
            SingleClick = 1,

            /// <summary>
            /// Double clicking a subitem or the primary column will edit that cell.
            /// F2 will edit the primary column.
            /// </summary>
            DoubleClick = 2,

            /// <summary>
            /// Pressing F2 is the only way to edit the cells. Once the primary column is being edited,
            /// the other cells in the row can be edited by pressing Tab.
            /// </summary>
            F2Only = 3
        }

        /// <summary>
        /// How does the user indicate that they want to edit a cell?
        /// None means that the listview cannot be edited.
        /// </summary>
        /// <remarks>Columns can also be marked as editable.</remarks>
        [Category("Behavior"),
        Description("How does the user indicate that they want to edit a cell?"),
        DefaultValue(CellEditActivateMode.None)]
        public CellEditActivateMode CellEditActivation
        {
            get { return cellEditActivation; }
            set { cellEditActivation = value; }
        }
        private CellEditActivateMode cellEditActivation = CellEditActivateMode.None;

        /// <summary>
        /// Should this list show checkboxes?
        /// </summary>
        public new bool CheckBoxes
        {
            get {
                return base.CheckBoxes;
            }
            set
            {
                base.CheckBoxes = value;
                // Initialize the state image list so we can display indetermined values.
                this.InitializeStateImageList();
            }
        }

        /// <summary>
        /// Return the model object of the row that is checked or null if no row is checked
        /// or more than one row is checked
        /// </summary>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Object CheckedObject
        {
            get {
                IList checkedObjects = this.CheckedObjects;
                if (checkedObjects.Count == 1)
                    return checkedObjects[0];
                else
                    return null;
            }
        }

        /// <summary>
        /// Get or set the collection of model objects that are checked.
        /// When setting this property, any row whose model object isn't
        /// in the given collection will be unchecked. Setting to null is
        /// equivilent to unchecking all.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property returns a simple collection. Changes made to the returned
        /// collection do NOT affect the list. This is different to the behaviour of
        /// CheckedIndicies collection.
        /// </para>
        /// <para>
        /// .NET's CheckedItems property is not helpful. It is just a short-hand for
        /// iterating through the list looking for items that are checked.
        /// </para>
        /// <para>
        /// The performance of this method is O(n). Be careful on long lists.
        /// </para>
        /// </remarks>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        virtual public IList CheckedObjects
        {
            get {
                ArrayList objects = new ArrayList();
                if (this.CheckBoxes) {
                    for (int i = 0; i < this.GetItemCount(); i++) {
                        OLVListItem olvi = this.GetItem(i);
                        if (olvi.Checked)
                            objects.Add(olvi.RowObject);
                    }
                }
                return objects;
            }
            set {
                if (this.CheckBoxes) {
                    if (value == null)
                        value = new ArrayList();

                    for (int i = 0; i < this.GetItemCount(); i++) {
                        OLVListItem olvi = this.GetItem(i);
                        bool newValue = value.Contains(olvi.RowObject);
                        if (olvi.Checked != newValue)
                            this.ChangeCheckItem(olvi, olvi.Checked, newValue);
                    }
                }
            }
        }

        /// <summary>
        /// Get/set the list of columns that should be used when the list switches to tile view.
        /// </summary>
        /// <remarks>If no list of columns has been installed, this value will default to the
        /// first column plus any column where IsTileViewColumn is true.</remarks>
        [Browsable(false),
        Obsolete("Use GetFilteredColumns() and OLVColumn.IsTileViewColumn instead"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<OLVColumn> ColumnsForTileView
        {
            get { return this.GetFilteredColumns(View.Tile); }
        }

        /// <summary>
        /// Return the visible columns in the order they are displayed to the user
        /// </summary>
        [Browsable(false)]
        public List<OLVColumn> ColumnsInDisplayOrder
        {
            get
            {
                List<OLVColumn> columnsInDisplayOrder = new List<OLVColumn>(this.Columns.Count);
                for (int i = 0; i < this.Columns.Count; i++)
                    columnsInDisplayOrder.Add(null);
                for (int i = 0; i < this.Columns.Count; i++) {
                    OLVColumn col = this.GetColumn(i);
                    columnsInDisplayOrder[col.DisplayIndex] = col;
                }
                return columnsInDisplayOrder;
            }
        }

        /// <summary>
        /// If there are no items in this list view, what message should be drawn onto the control?
        /// </summary>
        [Category("Appearance"),
         Description("When the list has no items, show this message in the control"),
         DefaultValue("")]
        public String EmptyListMsg
        {
            get { return emptyListMsg; }
            set
            {
                if (emptyListMsg != value) {
                    emptyListMsg = value;
                    this.Invalidate();
                }
            }
        }
        private String emptyListMsg = "";

        /// <summary>
        /// What font should the 'list empty' message be drawn in?
        /// </summary>
        [Category("Appearance"),
        Description("What font should the 'list empty' message be drawn in?"),
        DefaultValue(null)]
        public Font EmptyListMsgFont
        {
            get { return emptyListMsgFont; }
            set { emptyListMsgFont = value; }
        }
        private Font emptyListMsgFont;

        /// <summary>
        /// Return the font for the 'list empty' message or a default
        /// </summary>
        [Browsable(false)]
        public Font EmptyListMsgFontOrDefault
        {
            get
            {
                if (this.EmptyListMsgFont == null)
                    return new Font("Tahoma", 14);
                else
                    return this.EmptyListMsgFont;
            }
        }

        /// <summary>
        /// Get or set whether or not the listview is frozen. When the listview is
        /// frozen, it will not update itself.
        /// </summary>
        /// <remarks><para>The Frozen property is similar to the methods Freeze()/Unfreeze()
        /// except that changes to the Frozen property do not nest.</para></remarks>
        /// <example>objectListView1.Frozen = false; // unfreeze the control regardless of the number of Freeze() calls
        /// </example>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Frozen
        {
            get { return freezeCount > 0; }
            set
            {
                if (value)
                    Freeze();
                else if (freezeCount > 0) {
                    freezeCount = 1;
                    Unfreeze();
                }
            }
        }
        private int freezeCount = 0;

        /// <summary>
        /// When a group title has an item count, how should the lable be formatted?
        /// </summary>
        /// <remarks>
        /// The given format string can/should have two placeholders:
        /// <list type="bullet">
        /// <item>{0} - the original group title</item>
        /// <item>{1} - the number of items in the group</item>
        /// </list>
        /// </remarks>
        /// <example>"{0} [{1} items]"</example>
        [Category("Behavior"),
         Description("The format to use when suffixing item counts to group titles"),
         DefaultValue(null)]
        public string GroupWithItemCountFormat
        {
            get { return groupWithItemCountFormat; }
            set { groupWithItemCountFormat = value; }
        }
        private string groupWithItemCountFormat;

        /// <summary>
        /// Return this.GroupWithItemCountFormat or a reasonable default
        /// </summary>
        [Browsable(false)]
        public string GroupWithItemCountFormatOrDefault
        {
            get
            {
                if (String.IsNullOrEmpty(this.GroupWithItemCountFormat))
                    return "{0} [{1} items]";
                else
                    return this.GroupWithItemCountFormat;
            }
        }

        /// <summary>
        /// When a group title has an item count, how should the lable be formatted if
        /// there is only one item in the group?
        /// </summary>
        /// <remarks>
        /// The given format string can/should have two placeholders:
        /// <list type="bullet">
        /// <item>{0} - the original group title</item>
        /// <item>{1} - the number of items in the group (always 1)</item>
        /// </list>
        /// </remarks>
        /// <example>"{0} [{1} item]"</example>
        [Category("Behavior"),
         Description("The format to use when suffixing item counts to group titles"),
         DefaultValue(null)]
        public string GroupWithItemCountSingularFormat
        {
            get { return groupWithItemCountSingularFormat; }
            set { groupWithItemCountSingularFormat = value; }
        }
        private string groupWithItemCountSingularFormat;

        /// <summary>
        /// Return this.GroupWithItemCountSingularFormat or a reasonable default
        /// </summary>
        [Browsable(false)]
        public string GroupWithItemCountSingularFormatOrDefault
        {
            get
            {
                if (String.IsNullOrEmpty(this.GroupWithItemCountSingularFormat))
                    return "{0} [{1} item]";
                else
                    return this.GroupWithItemCountSingularFormat;
            }
        }

        /// <summary>
        /// Does this listview have a message that should be drawn when the list is empty?
        /// </summary>
        [Browsable(false)]
        public bool HasEmptyListMsg
        {
            get { return !String.IsNullOrEmpty(this.EmptyListMsg); }
        }

        /// <summary>
        /// What color should be used for the background of selected rows?
        /// </summary>
        /// <remarks>Windows does not give the option of changing the selection background.
        /// So this color is only used when control is owner drawn and when columns have a
        /// renderer installed -- a basic new BaseRenderer() will suffice.</remarks>
        [Category("Appearance"),
         Description("The background color of selected rows when the control is owner drawn"),
         DefaultValue(typeof(Color), "Empty")]
        public Color HighlightBackgroundColor
        {
            get { return highlightBackgroundColor; }
            set { highlightBackgroundColor = value; }
        }
        private Color highlightBackgroundColor = Color.Empty;

        /// <summary>
        /// Return the color should be used for the background of selected rows or a reasonable default
        /// </summary>
        [Browsable(false)]
        public Color HighlightBackgroundColorOrDefault
        {
            get  {
                if (this.HighlightBackgroundColor.IsEmpty)
                    return SystemColors.Highlight;
                else
                    return this.HighlightBackgroundColor;
            }
        }

        /// <summary>
        /// What color should be used for the foreground of selected rows?
        /// </summary>
        /// <remarks>Windows does not give the option of changing the selection foreground (text color).
        /// So this color is only used when control is owner drawn and when columns have a
        /// renderer installed -- a basic new BaseRenderer() will suffice.</remarks>
        [Category("Appearance"),
         Description("The foreground color of selected rows when the control is owner drawn"),
         DefaultValue(typeof(Color), "Empty")]
        public Color HighlightForegroundColor
        {
            get { return highlightForegroundColor; }
            set { highlightForegroundColor = value; }
        }
        private Color highlightForegroundColor = Color.Empty;

        /// <summary>
        /// Return the color should be used for the foreground of selected rows or a reasonable default
        /// </summary>
        [Browsable(false)]
        public Color HighlightForegroundColorOrDefault
        {
            get {
                if (this.HighlightForegroundColor.IsEmpty)
                    return SystemColors.HighlightText;
                else
                    return this.HighlightForegroundColor;
            }
        }

        /// <summary>
        /// Return true if a cell edit operation is currently happening
        /// </summary>
        [Browsable(false)]
        public bool IsCellEditing
        {
            get { return this.cellEditor != null; }
        }

        /// <summary>
        /// Which column did we last sort by
        /// </summary>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OLVColumn LastSortColumn
        {
            get { return lastSortColumn; }
            set { lastSortColumn = value; }
        }
        private OLVColumn lastSortColumn;

        /// <summary>
        /// Which direction did we last sort
        /// </summary>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SortOrder LastSortOrder
        {
            get { return lastSortOrder; }
            set { lastSortOrder = value; }
        }
        private SortOrder lastSortOrder;

        /// <summary>
        /// Get/set the collection of objects that this list will show
        /// </summary>
        /// <remarks>
        /// <para>
        /// The contents of the control will be updated immediately after setting this property</remarks>
        /// </para>
        /// <para>This method preserves selection, if possible. Use SetObjects() if
        /// you do not want to preserve the selection. Preserving selection is the slowest part of this
        /// code and performance is O(n) where n is the number of selected rows.</para>
        /// <para>This method is not thread safe.</para>
        /// </remarks>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEnumerable Objects
        {
            get { return this.objects; }
            set
            {
                this.BeginUpdate();
                try {
                    IList previousSelection = this.SelectedObjects;
                    this.SetObjects(value);
                    this.SelectedObjects = previousSelection;
                }
                finally {
                    this.EndUpdate();
                }
            }
        }
        private IEnumerable objects;

        /// <summary>
        /// Specify the height of each row in the control in pixels.
        /// </summary>
        /// <remarks><para>The row height in a listview is normally determined by the font size and the small image list size.
        /// This setting allows that calculation to be overridden (within reason: you still cannot set the line height to be
        /// less than the line height of the font used in the control). </para>
        /// <para>Setting it to -1 means use the normal calculation method.</para>
        /// <para><bold>This feature is experiemental!</bold> Strange things may happen to your program,
        /// your spouse or your pet if you use it.</para>
        /// </remarks>
        [Category("Appearance"),
         DefaultValue(-1)]
        public int RowHeight
        {
            get { return rowHeight; }
            set
            {
                if (value < 1)
                    rowHeight = -1;
                else
                    rowHeight = value;
                this.SetupExternalImageList();
            }
        }
        private int rowHeight = -1;

        /// <summary>
        /// Get/set the column that will be used to resolve comparisons that are equal when sorting.
        /// </summary>
        /// <remarks>There is no user interface for this setting. It must be set programmatically.
        /// The default is the first column.</remarks>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OLVColumn SecondarySortColumn
        {
            get
            {
                if (this.secondarySortColumn == null) {
                    if (this.Columns.Count > 0)
                        return this.GetColumn(0);
                    else
                        return null;
                } else
                    return this.secondarySortColumn;
            }
            set
            {
                this.secondarySortColumn = value;
            }
        }
        private OLVColumn secondarySortColumn;

        /// <summary>
        /// When the SecondarySortColumn is used, in what order will it compare results?
        /// </summary>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SortOrder SecondarySortOrder
        {
            get { return this.secondarySortOrder; }
            set { this.secondarySortOrder = value; }
        }
        private SortOrder secondarySortOrder = SortOrder.Ascending;

        /// <summary>
        /// When the user right clicks on the column headers, should a menu be presented which will allow
        /// them to choose which columns will be shown in the view?
        /// </summary>
        [Category("Behavior"),
        Description("When the user right clicks on the column headers, should a menu be presented which will allow them to choose which columns will be shown in the view?"),
        DefaultValue(true)]
        public bool SelectColumnsOnRightClick
        {
            get { return selectColumnsOnRightClick; }
            set { selectColumnsOnRightClick = value; }
        }
        private bool selectColumnsOnRightClick = true;

        /// <summary>
        /// When the column select menu is open, should it stay open after an item is selected?
        /// Staying open allows the user to turn more than one column on or off at a time.
        /// </summary>
        [Category("Behavior"),
        Description("When the column select menu is open, should it stay open after an item is selected?"),
        DefaultValue(true)]
        public bool SelectColumnsMenuStaysOpen
        {
            get { return selectColumnsMenuStaysOpen; }
            set { selectColumnsMenuStaysOpen = value; }
        }
        private bool selectColumnsMenuStaysOpen = true;

        /// <summary>
        /// Return the index of the row that is currently selected. If no row is selected,
        /// or more than one is selected, return -1.
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedIndex
        {
            get
            {
                if (this.SelectedIndices.Count == 1)
                    return this.SelectedIndices[0];
                else
                    return -1;
            }
            set
            {
                this.SelectedIndices.Clear();
                if (value >= 0 && value < this.Items.Count)
                    this.SelectedIndices.Add(value);
            }
        }

        /// <summary>
        /// Get the ListViewItem that is currently selected . If no row is selected, or more than one is selected, return null.
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ListViewItem SelectedItem
        {
            get
            {
                if (this.SelectedIndices.Count == 1)
                    return this.GetItem(this.SelectedIndices[0]);
                else
                    return null;
            }
            set
            {
                this.SelectedIndices.Clear();
                if (value != null)
                    this.SelectedIndices.Add(value.Index);
            }
        }

        /// <summary>
        /// Get the model object from the currently selected row. If no row is selected, or more than one is selected, return null.
        /// Select the row that is displaying the given model object. All other rows are deselected.
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Object SelectedObject
        {
            get { return this.GetSelectedObject(); }
            set { this.SelectObject(value); }
        }

        /// <summary>
        /// Get the model objects from the currently selected rows. If no row is selected, the returned List will be empty.
        /// When setting this value, select the rows that is displaying the given model objects. All other rows are deselected.
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList SelectedObjects
        {
            get { return this.GetSelectedObjects(); }
            set { this.SelectObjects(value); }
        }

        /// <summary>
        /// Should the list view show a bitmap in the column header to show the sort direction?
        /// </summary>
        /// <remarks>
        /// The only reason for not wanting to have sort indicators is that, on pre-XP versions of
        /// Windows, having sort indicators required the ListView to have a small image list, and
        /// as soon as you give a ListView a SmallImageList, the text of column 0 is bumped 16
        /// pixels to the right, even if you never used an image.
        /// </remarks>
        [Category("Behavior"),
         Description("Should the list view show sort indicators in the column headers?"),
         DefaultValue(true)]
        public bool ShowSortIndicators
        {
            get { return showSortIndicators; }
            set { showSortIndicators = value; }
        }
        private bool showSortIndicators;

        /// <summary>
        /// Should the list view show images on subitems?
        /// </summary>
        /// <remarks>
        /// <para>Under Windows, this works by sending messages to the underlying
        /// Windows control. To make this work under Mono, we would have to owner drawing the items :-(</para></remarks>
        [Category("Behavior"),
         Description("Should the list view show images on subitems?"),
         DefaultValue(false)]
        public bool ShowImagesOnSubItems
        {
            get
            {
#if MONO
                return false;
#else
                return showImagesOnSubItems;
#endif
            }
            set { showImagesOnSubItems = value; }
        }
        private bool showImagesOnSubItems;

        /// <summary>
        /// This property controls whether group labels will be suffixed with a count of items.
        /// </summary>
        /// <remarks>
        /// The format of the suffix is controlled by GroupWithItemCountFormat/GroupWithItemCountSingularFormat properties
        /// </remarks>
        [Category("Behavior"),
         Description("Will group titles be suffixed with a count of the items in the group?"),
         DefaultValue(false)]
        public bool ShowItemCountOnGroups
        {
            get { return showItemCountOnGroups; }
            set { showItemCountOnGroups = value; }
        }
        private bool showItemCountOnGroups;

        /// <summary>
        /// Override the SmallImageList property so we can correctly shadow its operations.
        /// </summary>
        /// <remarks><para>If you use the RowHeight property to specify the row height, the SmallImageList
        /// must be fully initialised before setting/changing the RowHeight. If you add new images to the image
        /// list after setting the RowHeight, you must assign the imagelist to the control again. Something as simple
        /// as this will work:
        /// <code>listView1.SmallImageList = listView1.SmallImageList;</code></para>
        /// </remarks>
        new public ImageList SmallImageList
        {
            get { return this.shadowedImageList; }
            set
            {
                this.shadowedImageList = value;
                this.SetupExternalImageList();
            }
        }
        private ImageList shadowedImageList = null;

        /// <summary>
        /// When the listview is grouped, should the items be sorted by the primary column?
        /// If this is false, the items will be sorted by the same column as they are grouped.
        /// </summary>
        [Category("Behavior"),
         Description("When the listview is grouped, should the items be sorted by the primary column? If this is false, the items will be sorted by the same column as they are grouped."),
         DefaultValue(true)]
        public bool SortGroupItemsByPrimaryColumn
        {
            get { return this.sortGroupItemsByPrimaryColumn; }
            set { this.sortGroupItemsByPrimaryColumn = value; }
        }
        private bool sortGroupItemsByPrimaryColumn = true;

        /// <summary>
        /// Get or set the index of the top item of this listview
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property only works when the listview is in Details view and not showing groups.
        /// </para>
        /// <para>
        /// The reason that it does not work when showing groups is that, when groups are enabled,
        /// the Windows message LVM_GETTOPINDEX always returns 0, regardless of the
        /// scroll position.
        /// </para>
        /// </remarks>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TopItemIndex
        {
            get
            {
                if (this.View == View.Details && this.TopItem != null)
                    return this.TopItem.Index;
                else
                    return -1;
            }
            set
            {
                int newTopIndex = Math.Min(value, this.GetItemCount() - 1);
                if (this.View == View.Details && newTopIndex >= 0) {
                    this.TopItem = this.Items[newTopIndex];

                    // Setting the TopItem sometimes gives off by one errors,
                    // that (bizarrely) are correct on a second attempt
                    if (this.TopItem != null && this.TopItem.Index != newTopIndex)
                        this.TopItem = this.GetItem(newTopIndex);
                }
            }
        }

        /// <summary>
        /// When resizing a column by dragging its divider, should any space filling columns be
        /// resized at each mouse move? If this is false, the filling columns will be
        /// updated when the mouse is released.
        /// </summary>
        /// <remarks>
        /// I think that this looks very ugly, but it does give more immediate feedback.
        /// It looks ugly because every
        /// column to the right of the divider being dragged gets updated twice: once when
        /// the column be resized changes size (this moves
        /// all the columns slightly to the right); then again when the filling columns are updated, but they will be shrunk
        /// so that the combined width is not more than the control, so everything jumps slightly back to the left again.
        /// </remarks>
        [Category("Behavior"),
        Description("When resizing a column by dragging its divider, should any space filling columns be resized at each mouse move?"),
        DefaultValue(false)]
        public bool UpdateSpaceFillingColumnsWhenDraggingColumnDivider
        {
            get { return updateSpaceFillingColumnsWhenDraggingColumnDivider; }
            set { updateSpaceFillingColumnsWhenDraggingColumnDivider = value; }
        }
        private bool updateSpaceFillingColumnsWhenDraggingColumnDivider = false;

        /// <summary>
        /// Should the list give a different background color to every second row?
        /// </summary>
        /// <remarks><para>The color of the alternate rows is given by AlternateRowBackColor.</para>
        /// <para>There is a "feature" in .NET for listviews in non-full-row-select mode, where
        /// selected rows are not drawn with their correct background color.</para></remarks>
        [Category("Appearance"),
         Description("Should the list view use a different backcolor to alternate rows?"),
         DefaultValue(false)]
        public bool UseAlternatingBackColors
        {
            get { return useAlternatingBackColors; }
            set { useAlternatingBackColors = value; }
        }
        private bool useAlternatingBackColors;

        /// <summary>
        /// Get/set the style of view that this listview is using
        /// </summary>
        /// <remarks>Switching to tile or details view installs the columns appropriate to that view.
        /// Confusingly, in tile view, every column is shown as a row of information.</remarks>
        new public View View
        {
            get { return base.View; }
            set
            {
                if (base.View == value)
                    return;

                if (this.Frozen) {
                    base.View = value;
                    return;
                }

                this.Freeze();

                // If we are switching to a Detail or Tile view, setup the columns needed for that view
                if (value == View.Details || value == View.Tile) {
                    this.ChangeToFilteredColumns(value);

                    if (value == View.Tile)
                        this.CalculateReasonableTileSize();
                }

                base.View = value;
                this.Unfreeze();
            }
        }

        #endregion

        #region Callbacks

        /// <summary>
        /// This delegate can be used to sort the table in a custom fasion.
        /// </summary>
        /// <remarks>
        /// <para>
        /// What the delegate has to do depends on the type of <code>ObjectListView</code> it is sorting:
        /// </para>
        /// <list>
        /// <item>
        /// If it is sorting a normal ObjectListView, the delegate must install a ListViewItemSorter on the ObjectListView. This install ItemSorter will actually do the work of sorting the ListViewItems. See ColumnComparer in the code for an example of what an ItemSorter has to do.
        /// </item>
        /// <item>
        /// If the delegate is sorting a VirtualObjectListView or a FastObjectListView, the delegate must sort the model objects that are sourcing the list (remember, in a virtual list, the application holds the model objects and the list just askes for them as it needs them).
        /// </item>
        /// </list>
        /// </remarks>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SortDelegate CustomSorter
        {
            get { return customSorter; }
            set { customSorter = value; }
        }
        private SortDelegate customSorter;

        /// <summary>
        /// This delegate can be used to format a OLVListItem before it is added to the control.
        /// </summary>
        /// <remarks>
        /// <para>The model object for the row can be found through the RowObject property of the OLVListItem object.</para>
        /// <para>All subitems normally have the same style as list item, so setting the forecolor on one
        /// subitem changes the forecolor of all subitems.
        /// To allow subitems to have different attributes, do this:<code>myListViewItem.UseItemStyleForSubItems = false;</code>.
        /// </para>
        /// <para>If UseAlternatingBackColors is true, the backcolor of the listitem will be calculated
        /// by the control and cannot be controlled by the RowFormatter delegate. In general, trying to use a RowFormatter
        /// when UseAlternatingBackColors is true does not work well.</para></remarks>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RowFormatterDelegate RowFormatter
        {
            get { return rowFormatter; }
            set { rowFormatter = value; }
        }
        private RowFormatterDelegate rowFormatter;

        /// <summary>
        /// This delegate will be called whenever the ObjectListView needs to know the check state
        /// of the row associated with a given model object. 
        /// </summary>
        /// <remarks>
        /// <para>.NET has limited support for indeterminate values. Returning null indicates the
        /// indeterminate value.</para>
        /// </remarks>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CheckStateGetterDelegate CheckStateGetter
        {
            get { return checkStateGetter; }
            set { checkStateGetter = value; }
        }
        private CheckStateGetterDelegate checkStateGetter;

        /// <summary>
        /// This delegate will be called whenever the user tries to change the check state
        /// of a row. The delegate should return the value that the listview should actuall
        /// use, which may be different to the one given to the delegate.
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CheckStatePutterDelegate CheckStatePutter
        {
            get { return checkStatePutter; }
            set { checkStatePutter = value; }
        }
        private CheckStatePutterDelegate checkStatePutter;

        #endregion

        #region List commands

        /// <summary>
        /// Add the given model object to this control.
        /// </summary>
        /// <param name="modelObject">The model object to be displayed</param>
        /// <remarks>See AddObjects() for more details</remarks>
        public void AddObject(object modelObject)
        {
            this.AddObjects(new object[] { modelObject });
        }

        /// <summary>
        /// Add the given collection of model objects to this control.
        /// </summary>
        /// <param name="modelObjects">A collection of model objects</param>
        /// <remarks>
        /// <para>The added objects will appear in their correct sort position, if sorting
        /// is active (i.e. if LastSortColumn is not null). Otherwise, they will appear at the end of the list.</para>
        /// <para>No check is performed to see if any of the objects are already in the ListView.</para>
        /// <para>Null objects are silently ignored.</para>
        /// </remarks>
        virtual public void AddObjects(ICollection modelObjects)
        {
            if (modelObjects == null)
                return;

            this.BeginUpdate();
            try {
                // Give the world a chance to cancel or change the added objects
                ItemsAddingEventArgs args = new ItemsAddingEventArgs(modelObjects);
                this.OnItemsAdding(args);
                if (args.Cancelled)
                    return;
                modelObjects = args.ObjectsToAdd;

                this.TakeOwnershipOfObjects();
                ArrayList ourObjects = (ArrayList)this.Objects;
                List<OLVListItem> itemList = new List<OLVListItem>();
                foreach (object modelObject in modelObjects) {
                    if (modelObject != null) {
                        ourObjects.Add(modelObject);
                        OLVListItem lvi = new OLVListItem(modelObject);
                        this.FillInValues(lvi, modelObject);
                        itemList.Add(lvi);
                    }
                }
                this.ListViewItemSorter = null;
                this.Items.AddRange(itemList.ToArray());
                this.Sort(this.lastSortColumn, this.lastSortOrder);

                foreach (OLVListItem lvi in itemList) {
                    this.SetSubItemImages(lvi.Index, lvi);
                }

                // Tell the world that the list has changed
                this.OnItemsChanged(new ItemsChangedEventArgs());
            }
            finally {
                this.EndUpdate();
            }
        }

        /// <summary>
        /// Organise the view items into groups, based on the last sort column or the first column
        /// if there is no last sort column
        /// </summary>
        public void BuildGroups()
        {
            this.BuildGroups(this.lastSortColumn);
        }

        /// <summary>
        /// Organise the view items into groups, based on the given column
        /// </summary>
        /// <remarks>If the AlwaysGroupByColumn property is not null,
        /// the list view items will be organisd by that column,
        /// and the 'column' parameter will be ignored.</remarks>
        /// <param name="column">The column whose values should be used for sorting.</param>
        virtual public void BuildGroups(OLVColumn column)
        {
            SortOrder order = this.lastSortOrder;
            if (order == SortOrder.None)
                order = this.Sorting;
            this.BuildGroups(column, order);
        }

        /// <summary>
        /// Organise the view items into groups, based on the given column
        /// </summary>
        /// <remarks>If the AlwaysGroupByColumn property is not null,
        /// the list view items will be organisd by that column,
        /// and the 'column' parameter will be ignored.</remarks>
        /// <param name="column">The column whose values should be used for sorting.</param>
        virtual public void BuildGroups(OLVColumn column, SortOrder order)
        {
            if (column == null)
                column = this.GetColumn(0);

            // If a specific column has been given as the group by column, we always
            // group by that column, regardless of what the user just clicked.
            OLVColumn groupByColumn = this.AlwaysGroupByColumn;
            if (groupByColumn == null)
                groupByColumn = column;

            SortOrder groupSortOrder = this.AlwaysGroupBySortOrder;
            if (groupSortOrder == SortOrder.None)
                groupSortOrder = order;

            this.Groups.Clear();

            // Getting the Count forces any internal cache of the ListView to be flushed. Without
            // this, iterating over the Items will not work correctly if the ListView handle
            // has not yet been created.
            int dummy = this.Items.Count;

            // Separate the list view items into groups, using the group key as the descrimanent
            Dictionary<object, List<OLVListItem>> map = new Dictionary<object, List<OLVListItem>>();
            foreach (OLVListItem olvi in this.Items) {
                object key = groupByColumn.GetGroupKey(olvi.RowObject);
                if (key == null)
                    key = "{null}"; // null can't be used as the key for a dictionary
                if (!map.ContainsKey(key))
                    map[key] = new List<OLVListItem>();
                map[key].Add(olvi);
            }

            // Make a list of the required groups
            List<ListViewGroup> groups = new List<ListViewGroup>();
            foreach (object key in map.Keys) {
                ListViewGroup lvg = new ListViewGroup(groupByColumn.ConvertGroupKeyToTitle(key));
                lvg.Tag = key;
                groups.Add(lvg);
            }

            // Sort the groups
            groups.Sort(new ListViewGroupComparer(groupSortOrder));

            // Put each group into the list view, and give each group its member items.
            // The order of statements is important here:
            // - the header must be calculate before the group is added to the list view,
            //   otherwise changing the header causes a nasty redraw (even in the middle of a BeginUpdate...EndUpdate pair)
            // - the group must be added before it is given items, otherwise an exception is thrown (is this documented?)
            string fmt = groupByColumn.GroupWithItemCountFormatOrDefault;
            string singularFmt = groupByColumn.GroupWithItemCountSingularFormatOrDefault;
            ColumnComparer itemSorter = new ColumnComparer((this.SortGroupItemsByPrimaryColumn ? this.GetColumn(0) : column),
                                                           order, this.SecondarySortColumn, this.SecondarySortOrder);
            foreach (ListViewGroup group in groups) {
                if (this.ShowItemCountOnGroups) {
                    int count = map[group.Tag].Count;
                    group.Header = String.Format((count == 1 ? singularFmt : fmt), group.Header, count);
                }
                this.Groups.Add(group);
                // If there is no sort order, don't sort since the sort isn't stable
                if (order != SortOrder.None)
                    map[group.Tag].Sort(itemSorter);
                group.Items.AddRange(map[group.Tag].ToArray());
            }
        }

        /// <summary>
        /// Build/rebuild all the list view items in the list
        /// </summary>
        virtual public void BuildList()
        {
            this.BuildList(true);
        }

        /// <summary>
        /// Build/rebuild all the list view items in the list
        /// </summary>
        /// <param name="shouldPreserveState">If this is true, the control will try to preserve the selection
        /// and the scroll position (see Remarks)
        /// </param>
        /// <remarks>
        /// <para>
        /// Use this method in situations were the contents of the list is basically the same
        /// as previously.
        /// </para>
        /// <para>
        /// Due to limitations in .NET's ListView, the scroll position is only preserved if
        /// the control is in Details view AND it is not showing groups.
        /// </para>
        /// </remarks>
        virtual public void BuildList(bool shouldPreserveState)
        {
            if (this.Frozen)
                return;

            IList previousSelection = new ArrayList();
            int previousTopIndex = this.TopItemIndex;
            if (shouldPreserveState && this.objects != null)
                previousSelection = this.SelectedObjects;

            this.BeginUpdate();
            try {
                this.Items.Clear();
                this.ListViewItemSorter = null;

                if (this.objects != null) {
                    // Build a list of all our items and then display them. (Building
                    // a list and then doing one AddRange is about 10-15% faster than individual adds)
                    List<OLVListItem> itemList = new List<OLVListItem>();
                    foreach (object rowObject in this.objects) {
                        OLVListItem lvi = new OLVListItem(rowObject);
                        this.FillInValues(lvi, rowObject);
                        itemList.Add(lvi);
                    }
                    this.Items.AddRange(itemList.ToArray());
                    this.SetAllSubItemImages();
                    this.Sort(this.lastSortColumn);

                    if (shouldPreserveState)
                        this.SelectedObjects = previousSelection;
                }
            }
            finally {
                this.EndUpdate();
            }

            // We can only restore the scroll position after the EndUpdate() because
            // of caching that the ListView does internally during a BeginUpdate/EndUpdate pair.
            if (shouldPreserveState)
                this.TopItemIndex = previousTopIndex;
        }

        /// <summary>
        /// Give the listview a reasonable size of its tiles, based on the number of lines of
        /// information that each tile is going to display.
        /// </summary>
        public void CalculateReasonableTileSize()
        {
            if (this.Columns.Count <= 0)
                return;

            int imageHeight = (this.LargeImageList == null ? 16 : this.LargeImageList.ImageSize.Height);
            int dataHeight = (this.Font.Height + 1) * this.Columns.Count;
            int tileWidth = (this.TileSize.Width == 0 ? 200 : this.TileSize.Width);
            int tileHeight = Math.Max(this.TileSize.Height, Math.Max(imageHeight, dataHeight));
            this.TileSize = new Size(tileWidth, tileHeight);
        }

        /// <summary>
        /// Rebuild this list for the given view
        /// </summary>
        /// <param name="view"></param>
        public void ChangeToFilteredColumns(View view)
        {
            // Store the state
            IList previousSelection = this.SelectedObjects;
            int previousTopIndex = this.TopItemIndex;

            this.Freeze();
            this.Clear();
            List<OLVColumn> cols = this.GetFilteredColumns(view);
            this.Columns.AddRange(cols.ToArray());
            if (view == View.Details) {
                foreach (OLVColumn x in cols) {
                    if (x.LastDisplayIndex == -1 || x.LastDisplayIndex > cols.Count - 1)
                        x.DisplayIndex = cols.Count - 1;
                    else
                        x.DisplayIndex = x.LastDisplayIndex;
                }
                this.ShowSortIndicator();
            }
            this.BuildList();
            this.Unfreeze();

            // Restore the state
            this.SelectedObjects = previousSelection;
            this.TopItemIndex = previousTopIndex;
        }

        /// <summary>
        /// Remove all items from this list
        /// </summary>
        /// <remark>This method can safely be called from background threads.</remark>
        virtual public void ClearObjects()
        {
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(ClearObjects));
            else
                this.Items.Clear();
        }

        /// <summary>
        /// Copy a text and html representation of the selected rows onto the clipboard.
        /// </summary>
        /// <remarks>Be careful when using this with virtual lists. If the user has selected
        /// 10,000,000 rows, this method will faithfully try to copy all of them to the clipboard.
        /// From the user's point of view, your program will appear to have hung.</remarks>
        public void CopySelectionToClipboard()
        {
            //THINK: Do we want to include something like this?
            //if (this.SelectedIndices.Count > 10000)
            //    return;

            this.CopyObjectsToClipboard(this.SelectedObjects);
        }

        /// <summary>
        /// Copy a text and html representation of the given objects onto the clipboard.
        /// </summary>
        public void CopyObjectsToClipboard(IList objectsToCopy)
        {
            if (objectsToCopy.Count == 0)
                return;

            List<OLVColumn> columns = this.ColumnsInDisplayOrder;

            // Build text and html versions of the selection
            StringBuilder sbText = new StringBuilder();
            StringBuilder sbHtml = new StringBuilder("<table>");

            foreach (object modelObject in objectsToCopy) {
                sbHtml.Append("<tr><td>");
                foreach (OLVColumn col in columns) {
                    if (col != columns[0]) {
                        sbText.Append("\t");
                        sbHtml.Append("</td><td>");
                    }
                    string strValue = col.GetStringValue(modelObject);
                    sbText.Append(strValue);
                    sbHtml.Append(strValue); //TODO: Should encode the string value
                }
                sbText.AppendLine();
                sbHtml.AppendLine("</td></tr>");
            }
            sbHtml.AppendLine("</table>");

            // Put both the text and html versions onto the clipboard
            DataObject dataObject = new DataObject();
            dataObject.SetText(sbText.ToString(), TextDataFormat.UnicodeText);
            dataObject.SetText(ConvertToHtmlFragment(sbHtml.ToString()), TextDataFormat.Html);
            Clipboard.SetDataObject(dataObject);
        }

        /// <summary>
        /// Convert the fragment of HTML into the Clipboards HTML format.
        /// </summary>
        /// <remarks>The HTML format is found here http://msdn2.microsoft.com/en-us/library/aa767917.aspx
        /// </remarks>
        /// <param name="fragment">The HTML to put onto the clipboard. It must be valid HTML!</param>
        /// <returns>A string that can be put onto the clipboard and will be recognized as HTML</returns>
        private string ConvertToHtmlFragment(string fragment)
        {
            // Minimal implementation of HTML clipboard format
            string source = "http://www.codeproject.com/KB/list/ObjectListView.aspx";

            const String MARKER_BLOCK =
                "Version:1.0\r\n" +
                "StartHTML:{0,8}\r\n" +
                "EndHTML:{1,8}\r\n" +
                "StartFragment:{2,8}\r\n" +
                "EndFragment:{3,8}\r\n" +
                "StartSelection:{2,8}\r\n" +
                "EndSelection:{3,8}\r\n" +
                "SourceURL:{4}\r\n" +
                "{5}";

            int prefixLength = String.Format(MARKER_BLOCK, 0, 0, 0, 0, source, "").Length;

            const String DEFAULT_HTML_BODY =
                "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">" +
                "<HTML><HEAD></HEAD><BODY><!--StartFragment-->{0}<!--EndFragment--></BODY></HTML>";

            string html = String.Format(DEFAULT_HTML_BODY, fragment);
            int startFragment = prefixLength + html.IndexOf(fragment);
            int endFragment = startFragment + fragment.Length;

            return String.Format(MARKER_BLOCK, prefixLength, prefixLength + html.Length, startFragment, endFragment, source, html);
        }

        /// <summary>
        /// Setup the list so it will draw selected rows using custom colours.
        /// </summary>
        /// <remarks>
        /// This method makes the list owner drawn, and ensures that all columns have at
        /// least a BaseRender installed.
        /// </remarks>
        public void EnableCustomSelectionColors()
        {
            this.OwnerDraw = true;

            foreach (OLVColumn column in this.AllColumns) {
                if (column.RendererDelegate == null)
                    column.Renderer = new BaseRenderer();
            }
        }

        /// <summary>
        /// Ensure that the given model object is visible
        /// </summary>
        /// <param name="modelObject">The model object to be revealed</param>
        public void EnsureModelVisible(Object modelObject)
        {
            int idx = this.IndexOf(modelObject);
            if (idx >= 0)
                this.EnsureVisible(idx);
        }

        /// <summary>
        /// Update the list to reflect the contents of the given collection, without affecting
        /// the scrolling position, selection or sort order.
        /// </summary>
        /// <param name="collection">The objects to be displayed</param>
        /// <remarks>
        /// <para>This method is about twice as slow as SetObjects().</para>
        /// <para>This method is experimental -- it may disappear in later versions of the code.</para>
        /// <para>There has to be a better way to do this! JPP 15/1/2008</para>
        /// <para>In most situations, if you need this functionality, use a FastObjectListView instead. JPP 2/2/2008</para>
        /// </remarks>
        [Obsolete("Use a FastObjectListView instead of this method.", false)]
        virtual public void IncrementalUpdate(IEnumerable collection)
        {
            if (this.InvokeRequired) {
                this.Invoke((MethodInvoker)delegate { this.IncrementalUpdate(collection); });
                return;
            }

            this.BeginUpdate();

            this.ListViewItemSorter = null;
            IList previousSelection = this.SelectedObjects;

            // Replace existing rows, creating new listviewitems if we get to the end of the list
            List<OLVListItem> newItems = new List<OLVListItem>();
            int rowIndex = 0;
            int itemCount = this.Items.Count;
            foreach (object model in collection) {
                if (rowIndex < itemCount) {
                    OLVListItem lvi = this.GetItem(rowIndex);
                    lvi.RowObject = model;
                    this.RefreshItem(lvi);
                } else {
                    OLVListItem lvi = new OLVListItem(model);
                    this.FillInValues(lvi, model);
                    newItems.Add(lvi);
                }
                rowIndex++;
            }

            // Delete any excess rows
            int numRowsToDelete = itemCount - rowIndex;
            for (int i = 0; i < numRowsToDelete; i++)
                this.Items.RemoveAt(rowIndex);

            this.Items.AddRange(newItems.ToArray());
            this.Sort(this.lastSortColumn);

            SetAllSubItemImages();

            this.SelectedObjects = previousSelection;

            this.EndUpdate();

            this.objects = collection;
        }

        /// <summary>
        /// Remove the given model object from the ListView
        /// </summary>
        /// <param name="modelObject">The model to be removed</param>
        /// <remarks>See RemoveObjects() for more details</remarks>
        public void RemoveObject(object modelObject)
        {
            this.RemoveObjects(new object[] { modelObject });
        }

        /// <summary>
        /// Pause (or unpause) all animations in the list
        /// </summary>
        /// <param name="isPause">true to pause, false to unpause</param>
        public void PauseAnimations(bool isPause)
        {
            for (int i = 0; i < this.Columns.Count; i++) {
                OLVColumn col = this.GetColumn(i);
                if (col.Renderer is ImageRenderer)
                    ((ImageRenderer)col.Renderer).Paused = isPause;
            }
        }

        /// <summary>
        /// Rebuild the columns based upon its current view and column visibility settings
        /// </summary>
        public void RebuildColumns()
        {
            this.ChangeToFilteredColumns(this.View);
        }

        /// <summary>
        /// Remove all of the given objects from the control
        /// </summary>
        /// <param name="modelObjects">Collection of objects to be removed</param>
        /// <remarks>
        /// <para>Nulls and model objects that are not in the ListView are silently ignored.</para>
        /// </remarks>
        virtual public void RemoveObjects(ICollection modelObjects)
        {
            if (modelObjects == null)
                return;

            this.BeginUpdate();
            try {
                // Give the world a chance to cancel or change the added objects
                ItemsRemovingEventArgs args = new ItemsRemovingEventArgs(modelObjects);
                this.OnItemsRemoving(args);
                if (args.Cancelled)
                    return;
                modelObjects = args.ObjectsToRemove;
                
                this.TakeOwnershipOfObjects();
                ArrayList ourObjects = (ArrayList)this.Objects;
                foreach (object modelObject in modelObjects) {
                    if (modelObject != null) {
                        ourObjects.Remove(modelObject);
                        int i = this.IndexOf(modelObject);
                        if (i >= 0)
                            this.Items.RemoveAt(i);
                    }
                }

                // Tell the world that the list has changed
                this.OnItemsChanged(new ItemsChangedEventArgs());
            }
            finally {
                this.EndUpdate();
            }
        }

        /// <summary>
        /// Set the collection of objects that will be shown in this list view.
        /// </summary>
        /// <remark>This method can safely be called from background threads.</remark>
        /// <remarks>The list is updated immediately</remarks>
        /// <param name="collection">The objects to be displayed</param>
        virtual public void SetObjects(IEnumerable collection)
        {
            if (this.InvokeRequired) {
                this.Invoke((MethodInvoker)delegate { this.SetObjects(collection); });
                return;
            }

            // Give the world a chance to cancel or change the assigned collection
            ItemsChangingEventArgs args = new ItemsChangingEventArgs(this.objects, collection);
            this.OnItemsChanging(args);
            if (args.Cancelled)
                return;
            collection = args.NewObjects;

            // If we own the current list and they change to another list, we don't own it anymore
            if (this.isOwnerOfObjects && this.objects != collection)
                this.isOwnerOfObjects = false;
            this.objects = collection;
            this.BuildList(false);

            // Tell the world that the list has changed
            this.OnItemsChanged(new ItemsChangedEventArgs());
        }

        /// <summary>
        /// Sort the items by the last sort column
        /// </summary>
        new public void Sort()
        {
            this.Sort(this.lastSortColumn);
        }

        #endregion

        #region Save/Restore State

        /// <summary>
        /// Return a byte array that represents the current state of the ObjectListView, such
        /// that the state can be restored by RestoreState()
        /// </summary>
        /// <remarks>
        /// <para>The state of an ObjectListView includes the attributes that the user can modify:
        /// <list>
        /// <item>current view (i.e. Details, Tile, Large Icon...)</item>
        /// <item>sort column and direction</item>
        /// <item>column order</item>
        /// <item>column widths</item>
        /// <item>column visibility</item>
        /// </list>
        /// </para>
        /// <para>
        /// It does not include selection or the scroll position.
        /// </para>
        /// </remarks>
        /// <returns>A byte array representing the state of the ObjectListView</returns>
        public byte[] SaveState()
        {
            ObjectListViewState olvState = new ObjectListViewState();
            olvState.VersionNumber = 1;
            olvState.NumberOfColumns = this.AllColumns.Count;
            olvState.CurrentView = this.View;

            // If we have a sort column, it is possible that it is not currently being shown, in which
            // case, it's Index will be -1. So we calculate its index directly. Technically, the sort
            // column does not even have to a member of AllColumns, in which case IndexOf will return -1,
            // which is works fine since we have no way of restoring such a column anyway.
            if (this.lastSortColumn != null)
                olvState.SortColumn = this.AllColumns.IndexOf(this.lastSortColumn);
            olvState.LastSortOrder = this.lastSortOrder;
            olvState.IsShowingGroups = this.ShowGroups;

            if (this.AllColumns.Count > 0 && this.AllColumns[0].LastDisplayIndex == -1)
                this.RememberDisplayIndicies();

            foreach (OLVColumn column in this.AllColumns) {
                olvState.ColumnIsVisible.Add(column.IsVisible);
                olvState.ColumnDisplayIndicies.Add(column.LastDisplayIndex);
                olvState.ColumnWidths.Add(column.Width);
            }

            // Now that we have stored our state, convert it to a byte array
            MemoryStream ms = new MemoryStream();
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(ms, olvState);

            return ms.ToArray();
        }

        /// <summary>
        /// Restore the state of the control from the given string, which must have been
        /// produced by SaveState()
        /// </summary>
        /// <param name="state">A byte array returned from SaveState()</param>
        /// <returns>Returns true if the state was restored</returns>
        public bool RestoreState(byte[] state)
        {
            MemoryStream ms = new MemoryStream(state);
            BinaryFormatter deserializer = new BinaryFormatter();
            ObjectListViewState olvState;
            try {
                olvState = deserializer.Deserialize(ms) as ObjectListViewState;
            } catch (System.Runtime.Serialization.SerializationException) {
                return false;
            }

            // The number of columns has changed. We have no way to match old
            // columns to the new ones, so we just give up.
            if (olvState.NumberOfColumns != this.AllColumns.Count)
                return false;

            if (olvState.SortColumn == -1) {
                this.lastSortColumn = null;
                this.lastSortOrder = SortOrder.None;
            } else {
                this.lastSortColumn = this.AllColumns[olvState.SortColumn];
                this.lastSortOrder = olvState.LastSortOrder;
            }

            for (int i = 0; i < olvState.NumberOfColumns; i++) {
                OLVColumn column = this.AllColumns[i];
                column.Width = (int)olvState.ColumnWidths[i];
                column.IsVisible = (bool)olvState.ColumnIsVisible[i];
                column.LastDisplayIndex = (int)olvState.ColumnDisplayIndicies[i];
            }

            if (olvState.IsShowingGroups != this.ShowGroups)
                this.ShowGroups = olvState.IsShowingGroups;

            if (this.View == olvState.CurrentView)
                this.RebuildColumns();
            else
                this.View = olvState.CurrentView;

            return true;
        }

        /// <summary>
        /// Instances of this class are used to store the state of an ObjectListView.
        /// </summary>
        [Serializable]
        internal class ObjectListViewState
        {
            public int VersionNumber = 1;
            public int NumberOfColumns = 1;
            public View CurrentView;
            public int SortColumn = -1;
            public bool IsShowingGroups;
            public SortOrder LastSortOrder = SortOrder.None;
            public ArrayList ColumnIsVisible = new ArrayList();
            public ArrayList ColumnDisplayIndicies = new ArrayList();
            public ArrayList ColumnWidths = new ArrayList();
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Event handler for the column click event
        /// </summary>
        virtual protected void HandleColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (!this.PossibleFinishCellEditing())
                return;

            // Toggle the sorting direction on successive clicks on the same column
            if (this.lastSortColumn != null && e.Column == this.lastSortColumn.Index)
                this.lastSortOrder = (this.lastSortOrder == SortOrder.Descending ? SortOrder.Ascending : SortOrder.Descending);
            else
                this.lastSortOrder = SortOrder.Ascending;

            this.BeginUpdate();
            try {
                this.Sort(e.Column);
            }
            finally {
                this.EndUpdate();
            }
        }

        /// <summary>
        /// Handle when a user checks/unchecks a row
        /// </summary>
        protected void HandleItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!this.Created || this.CheckStatePutter == null)
                return;

            Object modelObject = this.GetModelObject(e.Index);
            this.CheckStatePutter(modelObject, e.NewValue);

            if (this.CheckStateGetter != null) { 
                bool? state = this.CheckStateGetter(modelObject);
                if (state.HasValue)
                    if (state == true)
                        e.NewValue = CheckState.Checked;
                    else
                        e.NewValue = CheckState.Unchecked;
                else
                    e.NewValue = CheckState.Indeterminate;
            }
        }

        #endregion

        #region Low level Windows message handling

        /// <summary>
        /// Override the basic message pump for this control
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg) {
                case 0x0F: // WM_PAINT
                    this.HandlePrePaint();
                    base.WndProc(ref m);
                    this.HandlePostPaint();
                    break;
                case 0x4E: // WM_NOTIFY
                    if (!this.HandleNotify(ref m))
                        base.WndProc(ref m);
                    break;
                case 0x204E: // WM_REFLECT_NOTIFY
                    if (!this.HandleReflectNotify(ref m))
                        base.WndProc(ref m);
                    break;
                case 0x114: // WM_HSCROLL:
                case 0x115: // WM_VSCROLL:
                case 0x201: // WM_LBUTTONDOWN:
                case 0x20A: // WM_MOUSEWHEEL:
                case 0x20E: // WM_MOUSEHWHEEL:
                    if (this.PossibleFinishCellEditing())
                        base.WndProc(ref m);
                    break;
                case 0x7B: // WM_CONTEXTMENU
                    if (!this.HandleContextMenu(ref m))
                        base.WndProc(ref m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        #endregion

        #region Empty List Msg handling

        /// <summary>
        /// Perform any steps needed before painting the control
        /// </summary>
        protected void HandlePrePaint()
        {
            // When we get a WM_PAINT msg, remember the rectangle that is being updated.
            // We can't get this information later, since the BeginPaint call wipes it out.
            this.lastUpdateRectangle = NativeMethods.GetUpdateRect(this);

            // When the list is empty, we want to handle the drawing of the control by ourselves.
            // Unfortunately, there is no easy way to tell our superclass that we want to do this.
            // So we resort to guile and deception. We validate the list area of the control, which
            // effectively tells our superclass that this area does not need to be painted.
            // Our superclass will then not paint the control, leaving us free to do so ourselves.
            // Without doing this trickery, the superclass will draw the
            // list as empty, and then moments later, we will draw the empty msg, giving a nasty flicker
            if (this.GetItemCount() == 0 && this.HasEmptyListMsg)
                NativeMethods.ValidateRect(this, this.ClientRectangle);
        }

        /// <summary>
        /// Perform any steps needed after painting the control
        /// </summary>
        protected void HandlePostPaint()
        {
            // If the list isn't empty or there isn't an emptyList msg, do nothing
            if (this.GetItemCount() != 0 || !this.HasEmptyListMsg)
                return;

            // Draw the empty list msg centered in the client area of the control
            using (BufferedGraphics buffered = BufferedGraphicsManager.Current.Allocate(this.CreateGraphics(), this.ClientRectangle)) {
                Graphics g = buffered.Graphics;
                g.Clear(this.BackColor);
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                sf.Trimming = StringTrimming.EllipsisCharacter;
                g.DrawString(this.EmptyListMsg, this.EmptyListMsgFontOrDefault, SystemBrushes.ControlDark, this.ClientRectangle, sf);
                buffered.Render();
            }
        }

        #endregion

        #region Column header clicking, column hiding and resizing

        /// <summary>
        /// When the control is created capture the messages for the header.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
#if !MONO
            hdrCtrl = new HeaderControl(this);
#endif
        }
#if !MONO
        private HeaderControl hdrCtrl = null;

        /// <summary>
        /// Class used to capture window messages for the header of the list view
        /// control.
        /// </summary>
        /// <remarks>We only need this class in order to not change the cursor
        /// when the cursor is over the divider of a fixed width column. It
        /// really is a little too perfectionist even for me.</remarks>
        private class HeaderControl : NativeWindow
        {
            private ObjectListView parentListView = null;

            public HeaderControl(ObjectListView olv)
            {
                this.parentListView = olv;
                this.AssignHandle(NativeMethods.GetHeaderControl(olv));
            }

            /// <summary>
            /// Return the Windows handle behind this control
            /// </summary>
            /// <remarks>
            /// When an ObjectListView is initialized as part of a UserControl, the
            /// GetHeaderControl() method returns 0 until the UserControl is
            /// completely initialized. So the AssignHandle() call in the constructor
            /// doesn't work. So we override the Handle property so value is always
            /// current.
            /// </remarks>
            public new IntPtr Handle
            {
                get { return NativeMethods.GetHeaderControl(this.parentListView); }
            }

            protected override void WndProc(ref Message message)
            {
                const int WM_SETCURSOR = 0x0020;

                switch (message.Msg) {
                    case WM_SETCURSOR:
                        if (IsCursorOverLockedDivider()) {
                            message.Result = (IntPtr)1;	// Don't change the cursor
                            return;
                        }
                        break;
                }

                base.WndProc(ref message);
            }

            private bool IsCursorOverLockedDivider()
            {
                Point pt = this.parentListView.PointToClient(Cursor.Position);
                pt.X += NativeMethods.GetScrollPosition(this.parentListView.Handle, true);
                int dividerIndex = NativeMethods.GetDividerUnderPoint(this.Handle, pt);
                if (dividerIndex >= 0 && dividerIndex < this.parentListView.Columns.Count) {
                    OLVColumn column = this.parentListView.GetColumn(dividerIndex);
                    return column.IsFixedWidth || column.FillsFreeSpace;
                } else
                    return false;
            }

            /// <summary>
            /// Return the index of the column under the current cursor position,
            /// or -1 if the cursor is not over a column
            /// </summary>
            /// <returns>Index of the column under the cursor, or -1</returns>
            public int GetColumnIndexUnderCursor()
            {
                Point pt = this.parentListView.PointToClient(Cursor.Position);
                pt.X += NativeMethods.GetScrollPosition(this.parentListView.Handle, true);
                return NativeMethods.GetColumnUnderPoint(this.Handle, pt);
            }
        }
#endif
        /// <summary>
        /// The user wants to see the context menu.
        /// </summary>
        /// <param name="m">The windows message</param>
        /// <returns>A bool indicating if this message has been handled</returns>
        /// <remarks>
        /// We want to ignore context menu requests that are triggered by right clicks on the header
        /// </remarks>
        protected bool HandleContextMenu(ref Message m)
        {
            // Don't try to handle context menu commands at design time.
            if (this.DesignMode)
                return false;

            // If the context menu command was generated by the keyboard, LParam will be -1.
            // We don't want to process these.
            if (((int)m.LParam) == -1)
                return false;

            // If the context menu came from somewhere other than the header control,
            // we also don't want to ignore it
            if (m.WParam != this.hdrCtrl.Handle)
                return false;

            // OK. Looks like a right click in the header
            if (!this.PossibleFinishCellEditing())
                return true;

            int columnIndex = this.hdrCtrl.GetColumnIndexUnderCursor();
            return this.HandleHeaderRightClick(columnIndex);
        }

        /// <summary>
        /// In the notification messages, we handle attempts to change the width of our columns
        /// </summary>
        /// <param name="m">The msg to be processed</param>
        /// <returns>bool to indicate if the msg has been handled</returns>
        unsafe protected bool HandleReflectNotify(ref Message m)
        {
            const int LVN_ITEMCHANGED = -101;
            const int LVN_ITEMCHANGING = -100;
            const int LVIF_STATE = 8;

            bool isMsgHandled = false;
            NativeMethods.NMHDR nmhdr = (NativeMethods.NMHDR)m.GetLParam(typeof(NativeMethods.NMHDR));

            switch (nmhdr.code) {
                case LVN_ITEMCHANGED:
                    NativeMethods.NMLISTVIEW* nmlistviewPtr2 = (NativeMethods.NMLISTVIEW*)m.LParam;
                    if ((nmlistviewPtr2->uChanged & LVIF_STATE) != 0) {
                        CheckState currentValue = this.CalculateState(nmlistviewPtr2->uOldState);
                        CheckState newCheckValue = this.CalculateState(nmlistviewPtr2->uNewState);
                        if (currentValue != newCheckValue) {
                            ItemCheckedEventArgs args4 = new ItemCheckedEventArgs(this.Items[nmlistviewPtr2->iItem]);
                            this.OnItemChecked(args4);
                            // Remove the state indicies so that we don't trigger the OnItemChecked method 
                            // when we call our base method after exiting this method
                            nmlistviewPtr2->uOldState = (nmlistviewPtr2->uOldState & 0x0FFF);
                            nmlistviewPtr2->uNewState = (nmlistviewPtr2->uNewState & 0x0FFF);
                        }
                    }
                    break;

                case LVN_ITEMCHANGING:
                    // Change the CheckBox handling to cope with indeterminate state
                    NativeMethods.NMLISTVIEW* nmlistviewPtr = (NativeMethods.NMLISTVIEW*)m.LParam;
                    if ((nmlistviewPtr->uChanged & LVIF_STATE) != 0) {
                        CheckState currentValue = this.CalculateState(nmlistviewPtr->uOldState);
                        CheckState newCheckValue = this.CalculateState(nmlistviewPtr->uNewState);
                        if (currentValue != newCheckValue) {
                            ItemCheckEventArgs ice = new ItemCheckEventArgs(nmlistviewPtr->iItem, newCheckValue, currentValue);
                            this.OnItemCheck(ice);
                            if (ice.NewValue == currentValue)
                                m.Result = (IntPtr)1;
                            else {
                                m.Result = IntPtr.Zero;
                                // Within this message, we cannot change to any other value but the expected one
                                // So if we need to switch to another value, we use BeginInvole to change to the value
                                // we want just after this message completes.
                                if (ice.NewValue != newCheckValue) {
                                    OLVListItem olvItem = this.GetItem(nmlistviewPtr->iItem);
                                    this.BeginInvoke((MethodInvoker)delegate {
                                        olvItem.CheckState = ice.NewValue;
                                    });
                                }
                            }
                            //isMsgHandled = true;
                        }
                    }
                    break;

                default:
                    break;
            }

            return isMsgHandled;        
        }

        private CheckState CalculateState(int state)
        {
            switch ((state & 0xf000) >> 12) {
                case 1:
                    return CheckState.Unchecked;
                case 2:
                    return CheckState.Checked;
                case 3:
                    return CheckState.Indeterminate;
                default:
                    return CheckState.Checked;
            }
        }

        /// <summary>
        /// In the notification messages, we handle attempts to change the width of our columns
        /// </summary>
        /// <param name="m">The msg to be processed</param>
        /// <returns>bool to indicate if the msg has been handled</returns>
        unsafe protected bool HandleNotify(ref Message m)
        {
            bool isMsgHandled = false;

            const int HDN_FIRST = (0 - 300);
            const int HDN_ITEMCHANGINGA = (HDN_FIRST - 0);
            const int HDN_ITEMCHANGINGW = (HDN_FIRST - 20);
            const int HDN_ITEMCLICKA = (HDN_FIRST - 2);
            const int HDN_ITEMCLICKW = (HDN_FIRST - 22);
            const int HDN_DIVIDERDBLCLICKA = (HDN_FIRST - 5);
            const int HDN_DIVIDERDBLCLICKW = (HDN_FIRST - 25);
            const int HDN_BEGINTRACKA = (HDN_FIRST - 6);
            const int HDN_BEGINTRACKW = (HDN_FIRST - 26);
            //const int HDN_ENDTRACKA = (HDN_FIRST - 7);
            //const int HDN_ENDTRACKW = (HDN_FIRST - 27);
            const int HDN_TRACKA = (HDN_FIRST - 8);
            const int HDN_TRACKW = (HDN_FIRST - 28);

            // Handle the notification, remembering to handle both ANSI and Unicode versions
            NativeMethods.NMHDR nmhdr = (NativeMethods.NMHDR)m.GetLParam(typeof(NativeMethods.NMHDR));
            //if (nmhdr.code < HDN_FIRST)
            //    System.Diagnostics.Debug.WriteLine(nmhdr.code);

            // In KB Article #183258, MS states that when a header control has the HDS_FULLDRAG style, it will receive
            // ITEMCHANGING events rather than TRACK events. Under XP SP2 (at least) this is not always true, which may be
            // why MS has withdrawn that particular KB article. It is true that the header is always given the HDS_FULLDRAG
            // style. But even while window style set, the control doesn't always received ITEMCHANGING events.
            // The controlling setting seems to be the Explorer option "Show Window Contents While Dragging"!
            // In the category of "truly bizarre side effects", if the this option is turned on, we will receive
            // ITEMCHANGING events instead of TRACK events. But if it is turned off, we receive lots of TRACK events and
            // only one ITEMCHANGING event at the very end of the process.
            // If we receive HDN_TRACK messages, it's harder to control the resizing process. If we return a result of 1, we
            // cancel the whole drag operation, not just that particular track event, which is clearly not what we want.
            // If we are willing to compile with unsafe code enabled, we can modify the size of the column in place, using the
            // commented out code below. But without unsafe code, the best we can do is allow the user to drag the column to
            // any width, and then spring it back to within bounds once they release the mouse button. UI-wise it's very ugly.
            NativeMethods.NMHEADER nmheader;
            switch (nmhdr.code) {

                case HDN_ITEMCLICKA:
                case HDN_ITEMCLICKW:
                    if (!this.PossibleFinishCellEditing()) {
                        m.Result = (IntPtr)1; // prevent the change from happening
                        isMsgHandled = true;
                    }
                    break;

                case HDN_DIVIDERDBLCLICKA:
                case HDN_DIVIDERDBLCLICKW:
                case HDN_BEGINTRACKA:
                case HDN_BEGINTRACKW:
                    if (!this.PossibleFinishCellEditing()) {
                        m.Result = (IntPtr)1; // prevent the change from happening
                        isMsgHandled = true;
                        break;
                    }
                    nmheader = (NativeMethods.NMHEADER)m.GetLParam(typeof(NativeMethods.NMHEADER));
                    if (nmheader.iItem >= 0 && nmheader.iItem < this.Columns.Count) {
                        OLVColumn column = this.GetColumn(nmheader.iItem);
                        // Space filling columns can't be dragged or double-click resized
                        if (column.FillsFreeSpace) {
                            m.Result = (IntPtr)1; // prevent the change from happening
                            isMsgHandled = true;
                        }
                    }
                    break;

                case HDN_TRACKA:
                case HDN_TRACKW:
                    nmheader = (NativeMethods.NMHEADER)m.GetLParam(typeof(NativeMethods.NMHEADER));
                    if (nmheader.iItem >= 0 && nmheader.iItem < this.Columns.Count) {
                        NativeMethods.HDITEM* hditem = (NativeMethods.HDITEM*)nmheader.pHDITEM;
                        OLVColumn column = this.GetColumn(nmheader.iItem);
                        if (hditem->cxy < column.MinimumWidth)
                            hditem->cxy = column.MinimumWidth;
                        else if (column.MaximumWidth != -1 && hditem->cxy > column.MaximumWidth)
                            hditem->cxy = column.MaximumWidth;
                    }
                    break;

                case HDN_ITEMCHANGINGA:
                case HDN_ITEMCHANGINGW:
                    nmheader = (NativeMethods.NMHEADER)m.GetLParam(typeof(NativeMethods.NMHEADER));
                    if (nmheader.iItem >= 0 && nmheader.iItem < this.Columns.Count) {
                        NativeMethods.HDITEM hditem = (NativeMethods.HDITEM)Marshal.PtrToStructure(nmheader.pHDITEM, typeof(NativeMethods.HDITEM));
                        OLVColumn column = this.GetColumn(nmheader.iItem);
                        // Check the mask to see if the width field is valid, and if it is, make sure it's within range
                        if ((hditem.mask & 1) == 1) {
                            if (hditem.cxy < column.MinimumWidth ||
                                (column.MaximumWidth != -1 && hditem.cxy > column.MaximumWidth)) {
                                m.Result = (IntPtr)1; // prevent the change from happening
                                isMsgHandled = true;
                            }
                        }
                    }
                    break;

                default:
                    break;
            }

            return isMsgHandled;
        }


        /// <summary>
        /// The user has right clicked on the column headers. Do whatever is required
        /// </summary>
        /// <returns>Return true if this event has been handle</returns>
        virtual protected bool HandleHeaderRightClick(int columnIndex)
        {
            ColumnClickEventArgs eventArgs = new ColumnClickEventArgs(columnIndex);
            this.OnColumnRightClick(eventArgs);

            if (this.SelectColumnsOnRightClick)
                this.ShowColumnSelectMenu(Cursor.Position);

            return this.SelectColumnsOnRightClick;
        }

        /// <summary>
        /// The user has right clicked on the column headers. Do whatever is required
        /// </summary>
        /// <returns>Return true if this event has been handle</returns>
        [Obsolete("Use HandleHeaderRightClick(int) instead")]
        virtual protected bool HandleHeaderRightClick()
        {
            return false;
        }

        /// <summary>
        /// Tell the world when a cell is about to finish being edited.
        /// </summary>
        protected virtual void OnColumnRightClick(ColumnClickEventArgs e)
        {
            if (this.ColumnRightClick != null)
                this.ColumnRightClick(this, e);
        }

        /// <summary>
        /// The callbacks for RightColumnClick events
        /// </summary>
        public delegate void ColumnRightClickEventHandler(object sender, ColumnClickEventArgs e);

        /// <summary>
        /// Triggered when a column header is right clicked.
        /// </summary>
        [Category("Behavior")]
        public event ColumnRightClickEventHandler ColumnRightClick;

        /// <summary>
        /// Show a popup menu at the given point which will allow the user to choose which columns
        /// are visible on this listview
        /// </summary>
        /// <param name="pt">Where should the menu be placed</param>
        protected void ShowColumnSelectMenu(Point pt)
        {
            ContextMenuStrip m = this.MakeColumnSelectMenu(new ContextMenuStrip());
            m.Show(pt);
        }

        /// <summary>
        /// Append the column selection menu items to the given menu strip.
        /// </summary>
        /// <param name="strip">The menu to which the items will be added.</param>
        /// <returns>Return the menu to which the items were added</returns>
        public ContextMenuStrip MakeColumnSelectMenu(ContextMenuStrip strip)
        {
            strip.ItemClicked += new ToolStripItemClickedEventHandler(ColumnSelectMenu_ItemClicked);
            strip.Closing += new ToolStripDropDownClosingEventHandler(ColumnSelectMenu_Closing);

            List<OLVColumn> columns = new List<OLVColumn>(this.AllColumns);
            // Sort columns alphabetically
            //columns.Sort(delegate(OLVColumn x, OLVColumn y) { return String.Compare(x.Text, y.Text, true); });

            // Sort columns by display order
            if (this.AllColumns.Count > 0 && this.AllColumns[0].LastDisplayIndex == -1)
                this.RememberDisplayIndicies();
            columns.Sort(delegate(OLVColumn x, OLVColumn y) { return (x.LastDisplayIndex - y.LastDisplayIndex); });

            // Build menu from sorted columns
            foreach (OLVColumn col in columns) {
                ToolStripMenuItem mi = new ToolStripMenuItem(col.Text);
                mi.Checked = col.IsVisible;
                mi.Tag = col;

                // The 'Index' property returns -1 when the column is not visible, so if the
                // column isn't visible we have to enable the item. Also the first column can't be turned off
                mi.Enabled = !col.IsVisible || (col.Index > 0);
                strip.Items.Add(mi);
            }

            return strip;
        }

        private void ColumnSelectMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripMenuItem mi = (ToolStripMenuItem)e.ClickedItem;
            OLVColumn col = (OLVColumn)mi.Tag;
            mi.Checked = !mi.Checked;
            col.IsVisible = mi.Checked;
            this.BeginInvoke(new MethodInvoker(this.RebuildColumns));
        }

        private void ColumnSelectMenu_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            e.Cancel = (this.SelectColumnsMenuStaysOpen &&
                e.CloseReason == ToolStripDropDownCloseReason.ItemClicked);
        }

        /// <summary>
        /// Override the OnColumnReordered method to do what we want
        /// </summary>
        /// <param name="e"></param>
        protected override void OnColumnReordered(ColumnReorderedEventArgs e)
        {
            base.OnColumnReordered(e);

            // The internal logic of the .NET code behind a ENDDRAG event means that,
            // at this point, the DisplayIndex's of the columns are not yet as they are
            // going to be. So we have to invoke a method to run later that will remember
            // what the real DisplayIndex's are.
            this.BeginInvoke(new MethodInvoker(this.RememberDisplayIndicies));
        }

        private void RememberDisplayIndicies()
        {
            // Remember the display indexes so we can put them back at a later date
            foreach (OLVColumn x in this.AllColumns)
                x.LastDisplayIndex = x.DisplayIndex;
        }

        void HandleColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            if (this.UpdateSpaceFillingColumnsWhenDraggingColumnDivider && !this.GetColumn(e.ColumnIndex).FillsFreeSpace)
                this.ResizeFreeSpaceFillingColumns();
        }

        void HandleColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if (!this.GetColumn(e.ColumnIndex).FillsFreeSpace)
                this.ResizeFreeSpaceFillingColumns();
        }

        void HandleLayout(object sender, LayoutEventArgs e)
        {
            this.ResizeFreeSpaceFillingColumns();
        }

        /// <summary>
        /// Resize our space filling columns so they fill any unoccupied width in the control
        /// </summary>
        protected void ResizeFreeSpaceFillingColumns()
        {
            // It's too confusing to dynamically resize columns at design time.
            if (this.DesignMode)
                return;

            if (this.Frozen)
                return;

            // Calculate the free space available
            int freeSpace = this.ClientSize.Width - 2;
            int totalProportion = 0;
            List<OLVColumn> spaceFillingColumns = new List<OLVColumn>();
            for (int i = 0; i < this.Columns.Count; i++) {
                OLVColumn col = this.GetColumn(i);
                if (col.FillsFreeSpace) {
                    spaceFillingColumns.Add(col);
                    totalProportion += col.FreeSpaceProportion;
                } else
                    freeSpace -= col.Width;
            }
            freeSpace = Math.Max(0, freeSpace);

            // Any space filling column that would hit it's Minimum or Maximum
            // width must be treated as a fixed column.
            foreach (OLVColumn col in spaceFillingColumns.ToArray()) {
                int newWidth = (freeSpace * col.FreeSpaceProportion) / totalProportion;

                if (col.MinimumWidth != -1 && newWidth < col.MinimumWidth)
                    newWidth = col.MinimumWidth;
                else if (col.MaximumWidth != -1 && newWidth > col.MaximumWidth)
                    newWidth = col.MaximumWidth;
                else
                    newWidth = 0;

                if (newWidth > 0) {
                    col.Width = newWidth;
                    freeSpace -= newWidth;
                    totalProportion -= col.FreeSpaceProportion;
                    spaceFillingColumns.Remove(col);
                }
            }

            // Distribute the free space between the columns
            foreach (OLVColumn col in spaceFillingColumns) {
                col.Width = (freeSpace * col.FreeSpaceProportion) / totalProportion;
            }
        }

        #endregion

        #region Checkboxes

        /// <summary>
        /// Change the given item from the old check value to a new one
        /// </summary>
        /// <param name="olvi">The item to be change</param>
        /// <param name="oldValue">The old value of the check</param>
        /// <param name="newValue">The new value of the check</param>
        protected void ChangeCheckItem(OLVListItem olvi, bool oldValue, bool newValue)
        {
            this.ChangeCheckItem(olvi,
                (oldValue ? CheckState.Checked : CheckState.Unchecked),
                (newValue ? CheckState.Checked : CheckState.Unchecked));
        }

        /// <summary>
        /// Change the given item from the old check value to a new one
        /// </summary>
        /// <param name="olvi">The item to be change</param>
        /// <param name="oldValue">The old value of the check</param>
        /// <param name="newValue">The new value of the check</param>
        protected void ChangeCheckItem(OLVListItem olvi, CheckState oldValue, CheckState newValue)
        {
            olvi.CheckState = newValue;
            if (this.CheckStatePutter != null)
                this.CheckStatePutter(olvi.RowObject, newValue);
            this.RefreshItem(olvi);
        }

        /// <summary>
        /// Return true of the given object is checked
        /// </summary>
        /// <param name="modelObject">The model object whose checkedness is returned</param>
        /// <returns>Is the given object checked?</returns>
        /// <remarks>If the given object is not in the list, this method returns false.</remarks>
        public bool IsChecked(object modelObject)
        {
            OLVListItem olvi = this.ModelToItem(modelObject);
            if (olvi == null)
                return false;
            else
                return olvi.Checked;
        }

        /// <summary>
        /// Return the OLVListItem that displays the given model object
        /// </summary>
        /// <param name="modelObject">The modelObject whose item is to be found</param>
        /// <returns>The OLVListItem that displays the model, or null</returns>
        /// <remarks>This method has O(n) performance.</remarks>
        protected OLVListItem ModelToItem(object modelObject)
        {
            if (modelObject == null)
                return null;

            OLVListItem olvi;
            foreach (ListViewItem lvi in this.Items) {
                olvi = (OLVListItem)lvi;
                if (olvi.RowObject == modelObject)
                    return olvi;
            }
            return null;
        }

        /// <summary>
        /// Toggle the checkedness of the given object
        /// </summary>
        /// <param name="modelObject">The model object to be checked</param>
        public void ToggleCheckObject(object modelObject)
        {
            OLVListItem olvi = this.ModelToItem(modelObject);
            if (olvi != null)
                this.ChangeCheckItem(olvi, olvi.Checked, !olvi.Checked);
        }

        /// <summary>
        /// Mark the given object as checked in the list
        /// </summary>
        /// <param name="modelObject">The model object to be checked</param>
        public void CheckObject(object modelObject)
        {
            OLVListItem olvi = this.ModelToItem(modelObject);
            if (olvi != null && !olvi.Checked)
                this.ChangeCheckItem(olvi, false, true);
        }

        /// <summary>
        /// Mark the given object as unchecked in the list
        /// </summary>
        /// <param name="modelObject">The model object to be unchecked</param>
        public void UncheckObject(object modelObject)
        {
            OLVListItem olvi = this.ModelToItem(modelObject);
            if (olvi != null && olvi.Checked)
                this.ChangeCheckItem(olvi, true, false);
        }

        #endregion

        #region OLV accessing

        /// <summary>
        /// Return the column at the given index
        /// </summary>
        /// <param name="index">Index of the column to be returned</param>
        /// <returns>An OLVColumn</returns>
        public OLVColumn GetColumn(int index)
        {
            return (OLVColumn)this.Columns[index];
        }

        /// <summary>
        /// Return the column at the given title.
        /// </summary>
        /// <param name="name">Name of the column to be returned</param>
        /// <returns>An OLVColumn</returns>
        public OLVColumn GetColumn(string name)
        {
            foreach (ColumnHeader column in this.Columns) {
                if (column.Text == name)
                    return (OLVColumn)column;
            }
            return null;
        }

        /// <summary>
        /// Return the number of items in the list
        /// </summary>
        /// <returns>the number of items in the list</returns>
        virtual public int GetItemCount()
        {
            return this.Items.Count;
        }

        /// <summary>
        /// Return the item at the given index
        /// </summary>
        /// <param name="index">Index of the item to be returned</param>
        /// <returns>An OLVListItem</returns>
        virtual public OLVListItem GetItem(int index)
        {
            if (index >= 0 && index < this.GetItemCount())
                return (OLVListItem)this.Items[index];
            else
                return null;
        }

        /// <summary>
        /// Return the model object at the given index
        /// </summary>
        /// <param name="index">Index of the model object to be returned</param>
        /// <returns>A model object</returns>
        virtual public object GetModelObject(int index)
        {
            return this.GetItem(index).RowObject;
        }

        /// <summary>
        /// Find the item and column that are under the given co-ords
        /// </summary>
        /// <param name="x">X co-ord</param>
        /// <param name="y">Y co-ord</param>
        /// <param name="selectedColumn">The column under the given point</param>
        /// <returns>The item under the given point. Can be null.</returns>
        public OLVListItem GetItemAt(int x, int y, out OLVColumn selectedColumn)
        {
            selectedColumn = null;
            ListViewHitTestInfo info = this.HitTest(x, y);
            if (info.Item == null)
                return null;

            if (info.SubItem != null) {
                int subItemIndex = info.Item.SubItems.IndexOf(info.SubItem);
                selectedColumn = this.GetColumn(subItemIndex);
            }

            return (OLVListItem)info.Item;
        }

        #endregion

        #region Object manipulation

        /// <summary>
        /// Select all rows in the listview
        /// </summary>
        public void SelectAll()
        {
            NativeMethods.SelectAllItems(this);
        }

        /// <summary>
        /// Deselect all rows in the listview
        /// </summary>
        public void DeselectAll()
        {
            NativeMethods.DeselectAllItems(this);
        }

        /// <summary>
        /// Return the model object of the row that is selected or null if there is no selection or more than one selection
        /// </summary>
        /// <returns>Model object or null</returns>
        virtual public object GetSelectedObject()
        {
            if (this.SelectedIndices.Count == 1)
                return this.GetModelObject(this.SelectedIndices[0]);
            else
                return null;
        }

        /// <summary>
        /// Return the model objects of the rows that are selected or an empty collection if there is no selection
        /// </summary>
        /// <returns>ArrayList</returns>
        virtual public ArrayList GetSelectedObjects()
        {
            ArrayList objects = new ArrayList(this.SelectedIndices.Count);
            foreach (int index in this.SelectedIndices)
                objects.Add(this.GetModelObject(index));

            return objects;
        }

        /// <summary>
        /// Return the model object of the row that is checked or null if no row is checked
        /// or more than one row is checked
        /// </summary>
        /// <returns>Model object or null</returns>
        /// <remarks>Use CheckedObject property instead of this method</remarks>
        [Obsolete("Use CheckedObject property instead of this method")]
        virtual public object GetCheckedObject()
        {
            return this.CheckedObject;
        }

        /// <summary>
        /// Get the collection of model objects that are checked.
        /// </summary>
        /// <remarks>Use CheckedObjects property instead of this method</remarks>
        [Obsolete("Use CheckedObjects property instead of this method")]
        virtual public ArrayList GetCheckedObjects()
        {
            return (ArrayList)this.CheckedObjects;
        }

        /// <summary>
        /// Select the row that is displaying the given model object. All other rows are deselected.
        /// </summary>
        /// <param name="modelObject">The object to be selected or null to deselect all</param>
        virtual public void SelectObject(object modelObject)
        {
            this.SelectObject(modelObject, false);
        }

        /// <summary>
        /// Select the row that is displaying the given model object. All other rows are deselected.
        /// </summary>
        /// <param name="modelObject">The object to be selected or null to deselect all</param>
        /// <param name="setFocus">Should the object be focused as well?</param>
        virtual public void SelectObject(object modelObject, bool setFocus)
        {
            // If the given model is already selected, don't do anything else (prevents an flicker)
            if (this.SelectedItems.Count == 1 && ((OLVListItem)this.SelectedItems[0]).RowObject == modelObject)
                return;

            this.SelectedItems.Clear();

            OLVListItem olvi = this.ModelToItem(modelObject);
            if (olvi != null) {
                olvi.Selected = true;
                if (setFocus)
                    olvi.Focused = true;
            }
        }

        /// <summary>
        /// Select the rows that is displaying any of the given model object. All other rows are deselected.
        /// </summary>
        /// <param name="modelObjects">A collection of model objects</param>
        virtual public void SelectObjects(IList modelObjects)
        {
            this.SelectedItems.Clear();

            if (modelObjects == null)
            	return;
            
            foreach (object modelObject in modelObjects) {
                OLVListItem olvi = this.ModelToItem(modelObject);
                if (olvi != null)
                    olvi.Selected = true;
            }
        }

        /// <summary>
        /// Update the ListViewItem with the data from its associated model.
        /// </summary>
        /// <remarks>This method does not resort or regroup the view. It simply updates
        /// the displayed data of the given item</remarks>
        virtual public void RefreshItem(OLVListItem olvi)
        {
            // For some reason, clearing the subitems also wipes out the back color,
            // so we need to store it and then put it back again later
            Color c = olvi.BackColor;
            olvi.SubItems.Clear();
            this.FillInValues(olvi, olvi.RowObject);
            this.SetSubItemImages(olvi.Index, olvi, true);
            olvi.BackColor = c;
        }

        /// <summary>
        /// Update the rows that are showing the given objects
        /// </summary>
        /// <remarks>This method does not resort or regroup the view.</remarks>
        virtual public void RefreshObject(object modelObject)
        {
            this.RefreshObjects(new object[] { modelObject });
        }

        /// <summary>
        /// Update the rows that are showing the given objects
        /// </summary>
        /// <remarks>
        /// <para>This method does not resort or regroup the view.</para>
        /// <para>This method can safely be called from background threads.</para>
        /// </remarks>
        virtual public void RefreshObjects(IList modelObjects)
        {
            if (this.InvokeRequired) {
                this.Invoke((MethodInvoker)delegate { this.RefreshObjects(modelObjects); });
                return;
            }
            foreach (object modelObject in modelObjects) {
                OLVListItem olvi = this.ModelToItem(modelObject);
                if (olvi != null)
                    this.RefreshItem(olvi);
            }
        }

        /// <summary>
        /// Update the rows that are selected
        /// </summary>
        /// <remarks>This method does not resort or regroup the view.</remarks>
        public void RefreshSelectedObjects()
        {
            foreach (ListViewItem lvi in this.SelectedItems)
                this.RefreshItem((OLVListItem)lvi);
        }

        /// <summary>
        /// Find the given model object within the listview and return its index
        /// </summary>
        /// <remarks>Technically, this method will work with virtual lists, but it will
        /// probably be very slow.</remarks>
        /// <param name="modelObject">The model object to be found</param>
        /// <returns>The index of the object. -1 means the object was not present</returns>
        public int IndexOf(Object modelObject)
        {
            for (int i = 0; i < this.GetItemCount(); i++) {
                if (this.GetModelObject(i) == modelObject)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Return the ListViewItem that appears immediately after the given item.
        /// If the given item is null, the first item in the list will be returned.
        /// Return null if the given item is the last item.
        /// </summary>
        /// <param name="itemToFind">The item that is before the item that is returned, or null</param>
        /// <returns>A ListViewItem</returns>
        public ListViewItem GetNextItem(ListViewItem itemToFind)
        {
            if (this.ShowGroups) {
                bool isFound = (itemToFind == null);
                foreach (ListViewGroup group in this.Groups) {
                    foreach (ListViewItem lvi in group.Items) {
                        if (isFound)
                            return lvi;
                        isFound = (lvi == itemToFind);
                    }
                }
                return null;
            } else {
                if (this.GetItemCount() == 0)
                    return null;
                if (itemToFind == null)
                    return this.GetItem(0);
                if (itemToFind.Index == this.GetItemCount() - 1)
                    return null;
                return this.GetItem(itemToFind.Index + 1);
            }
        }

        /// <summary>
        /// Return the ListViewItem that appears immediately before the given item.
        /// If the given item is null, the last item in the list will be returned.
        /// Return null if the given item is the first item.
        /// </summary>
        /// <param name="itemToFind">The item that is before the item that is returned</param>
        /// <returns>A ListViewItem</returns>
        public ListViewItem GetPreviousItem(ListViewItem itemToFind)
        {
            if (this.ShowGroups) {
                ListViewItem previousItem = null;
                foreach (ListViewGroup group in this.Groups) {
                    foreach (ListViewItem lvi in group.Items) {
                        if (lvi == itemToFind)
                            return previousItem;
                        else
                            previousItem = lvi;
                    }
                }
                if (itemToFind == null)
                    return previousItem;
                else
                    return null;
            } else {
                if (this.GetItemCount() == 0)
                    return null;
                if (itemToFind == null)
                    return this.GetItem(this.GetItemCount() - 1);
                if (itemToFind.Index == 0)
                    return null;
                return this.GetItem(itemToFind.Index - 1);
            }
        }

        #endregion

        #region Freezing

        /// <summary>
        /// Freeze the listview so that it no longer updates itself.
        /// </summary>
        /// <remarks>Freeze()/Unfreeze() calls nest correctly</remarks>
        public void Freeze()
        {
            freezeCount++;
        }

        /// <summary>
        /// Unfreeze the listview. If this call is the outermost Unfreeze(),
        /// the contents of the listview will be rebuilt.
        /// </summary>
        /// <remarks>Freeze()/Unfreeze() calls nest correctly</remarks>
        public void Unfreeze()
        {
            if (freezeCount <= 0)
                return;

            freezeCount--;
            if (freezeCount == 0)
                DoUnfreeze();
        }

        /// <summary>
        /// Do the actual work required when the listview is unfrozen
        /// </summary>
        virtual protected void DoUnfreeze()
        {
            this.ResizeFreeSpaceFillingColumns();
            this.BuildList();
        }

        #endregion

        #region Column Sorting

        /// <summary>
        /// Sort the items in the list view by the values in the given column.
        /// If ShowGroups is true, the rows will be grouped by the given column,
        /// otherwise, it will be a straight sort.
        /// </summary>
        /// <param name="columnToSortName">The name of the column whose values will be used for the sorting</param>
        public void Sort(string columnToSortName)
        {
            this.Sort(this.GetColumn(columnToSortName));
        }

        /// <summary>
        /// Sort the items in the list view by the values in the given column.
        /// If ShowGroups is true, the rows will be grouped by the given column,
        /// otherwise, it will be a straight sort.
        /// </summary>
        /// <param name="columnToSortIndex">The index of the column whose values will be used for the sorting</param>
        public void Sort(int columnToSortIndex)
        {
            if (columnToSortIndex >= 0 && columnToSortIndex < this.Columns.Count)
                this.Sort(this.GetColumn(columnToSortIndex));
        }

        /// <summary>
        /// Sort the items in the list view by the values in the given column.
        /// If ShowGroups is true, the rows will be grouped by the given column,
        /// otherwise, it will be a straight sort.
        /// </summary>
        /// <param name="columnToSort">The column whose values will be used for the sorting</param>
        virtual public void Sort(OLVColumn columnToSort)
        {
            if (this.InvokeRequired) {
                this.Invoke((MethodInvoker)delegate { this.Sort(columnToSort); });
                return;
            }

            SortOrder order = this.lastSortOrder;
            if (order == SortOrder.None)
                order = this.Sorting;

            this.Sort(columnToSort, order);
        }
        /// <summary>
        /// Sort the items in the list view by the values in the given column.
        /// If ShowGroups is true, the rows will be grouped by the given column,
        /// otherwise, it will be a straight sort.
        /// </summary>
        /// <param name="columnToSort">The column whose values will be used for the sorting</param>
        virtual public void Sort(OLVColumn columnToSort, SortOrder order)
        {
            if (this.InvokeRequired) {
                this.Invoke((MethodInvoker)delegate { this.Sort(columnToSort, order); });
                return;
            }

            // Sanity checks
            if (columnToSort == null || order == SortOrder.None || this.Columns.Count < 1)
                return;

            BeforeSortingEventArgs args = new BeforeSortingEventArgs(columnToSort, order);
            this.OnBeforeSorting(args);
            if (args.Cancelled)
                return;
            
            // The event handler may have changed the sorting pattern
            columnToSort = args.ColumnToSort;
            order = args.SortOrder;

            if (this.ShowGroups)
                this.BuildGroups(columnToSort, order);
            else if (this.CustomSorter != null)
                this.CustomSorter(columnToSort, order);
            else
                this.ListViewItemSorter = new ColumnComparer(columnToSort, order, this.SecondarySortColumn, this.SecondarySortOrder);

            if (this.ShowSortIndicators)
                this.ShowSortIndicator(columnToSort, order);

            if (this.UseAlternatingBackColors && this.View == View.Details)
                PrepareAlternateBackColors();

            this.lastSortColumn = columnToSort;
            this.lastSortOrder = order;

            this.OnAfterSorting(new AfterSortingEventArgs(columnToSort, order));
        }


        /// <summary>
        /// Put a sort indicator next to the text of the sort column
        /// </summary>
        public void ShowSortIndicator()
        {
            if (this.ShowSortIndicators && this.lastSortOrder != SortOrder.None)
                this.ShowSortIndicator(this.lastSortColumn, this.lastSortOrder);
        }


        /// <summary>
        /// Put a sort indicator next to the text of the given given column
        /// </summary>
        /// <param name="columnToSort">The column to be marked</param>
        /// <param name="sortOrder">The sort order in effect on that column</param>
        protected void ShowSortIndicator(OLVColumn columnToSort, SortOrder sortOrder)
        {
            int imageIndex = -1;

            if (!NativeMethods.HasBuiltinSortIndicators()) {
                // If we can't use builtin image, we have to make and then locate the index of the
                // sort indicator we want to use. SortOrder.None doesn't show an image.
                if (this.SmallImageList == null || !this.SmallImageList.Images.ContainsKey(SORT_INDICATOR_UP_KEY))
                    MakeSortIndicatorImages();

                if (sortOrder == SortOrder.Ascending)
                    imageIndex = this.SmallImageList.Images.IndexOfKey(SORT_INDICATOR_UP_KEY);
                else if (sortOrder == SortOrder.Descending)
                    imageIndex = this.SmallImageList.Images.IndexOfKey(SORT_INDICATOR_DOWN_KEY);
            }

            // Set the image for each column
            for (int i = 0; i < this.Columns.Count; i++) {
                if (i == columnToSort.Index)
                    NativeMethods.SetColumnImage(this, i, sortOrder, imageIndex);
                else
                    NativeMethods.SetColumnImage(this, i, SortOrder.None, -1);
            }
        }

        /// <summary>
        /// The name of the image used when a column is sorted ascending
        /// </summary>
        /// <remarks>This image is only used on pre-XP systems. System images are used for XP and later</remarks>
        public const string SORT_INDICATOR_UP_KEY = "sort-indicator-up";

        /// <summary>
        /// The name of the image used when a column is sorted descending
        /// </summary>
        /// <remarks>This image is only used on pre-XP systems. System images are used for XP and later</remarks>
        public const string SORT_INDICATOR_DOWN_KEY = "sort-indicator-down";

        /// <summary>
        /// If the sort indicator images don't already exist, this method will make and install them
        /// </summary>
        protected void MakeSortIndicatorImages()
        {
            ImageList il = this.SmallImageList;
            if (il == null) {
                il = new ImageList();
                il.ImageSize = new Size(16, 16);
            }

            // This arrangement of points works well with (16,16) images, and OK with others
            int midX = il.ImageSize.Width / 2;
            int midY = (il.ImageSize.Height / 2) - 1;
            int deltaX = midX - 2;
            int deltaY = deltaX / 2;

            if (il.Images.IndexOfKey(SORT_INDICATOR_UP_KEY) == -1) {
                Point pt1 = new Point(midX - deltaX, midY + deltaY);
                Point pt2 = new Point(midX, midY - deltaY - 1);
                Point pt3 = new Point(midX + deltaX, midY + deltaY);
                il.Images.Add(SORT_INDICATOR_UP_KEY, this.MakeTriangleBitmap(il.ImageSize, new Point[] { pt1, pt2, pt3 }));
            }

            if (il.Images.IndexOfKey(SORT_INDICATOR_DOWN_KEY) == -1) {
                Point pt1 = new Point(midX - deltaX, midY - deltaY);
                Point pt2 = new Point(midX, midY + deltaY);
                Point pt3 = new Point(midX + deltaX, midY - deltaY);
                il.Images.Add(SORT_INDICATOR_DOWN_KEY, this.MakeTriangleBitmap(il.ImageSize, new Point[] { pt1, pt2, pt3 }));
            }

            this.SmallImageList = il;
        }

        private Bitmap MakeTriangleBitmap(Size sz, Point[] pts)
        {
            Bitmap bm = new Bitmap(sz.Width, sz.Height);
            Graphics g = Graphics.FromImage(bm);
            g.FillPolygon(new SolidBrush(Color.Gray), pts);
            return bm;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// For some reason, UseItemStyleForSubItems doesn't work for the colors
        /// when owner drawing the list, so we have to specifically give each subitem
        /// the desired colors
        /// </summary>
        /// <param name="olvi">The item whose subitems are to be corrected</param>
        protected void CorrectSubItemColors(ListViewItem olvi)
        {
            if (this.OwnerDraw && olvi.UseItemStyleForSubItems)
                foreach (ListViewItem.ListViewSubItem si in olvi.SubItems) {
                    si.BackColor = olvi.BackColor;
                    si.ForeColor = olvi.ForeColor;
                }
        }

        /// <summary>
        /// Fill in the given OLVListItem with values of the given row
        /// </summary>
        /// <param name="lvi">the OLVListItem that is to be stuff with values</param>
        /// <param name="rowObject">the model object from which values will be taken</param>
        protected void FillInValues(OLVListItem lvi, object rowObject)
        {
            if (this.Columns.Count == 0)
                return;

            OLVColumn column = this.GetColumn(0);
            lvi.Text = column.GetStringValue(rowObject);
            lvi.ImageSelector = column.GetImage(rowObject);

            for (int i = 1; i < this.Columns.Count; i++) {
                column = this.GetColumn(i);
                lvi.SubItems.Add(new OLVListSubItem(column.GetStringValue(rowObject),
                                                    column.GetImage(rowObject)));
            }

            // Give the row formatter a chance to mess with the item
            if (this.RowFormatter != null)
                this.RowFormatter(lvi);

            this.CorrectSubItemColors(lvi);

            // Get the check state of the row, if we are showing check boxes
            if (this.CheckBoxes && this.CheckStateGetter != null)
                lvi.SetCheckState(this.CheckStateGetter(lvi.RowObject));
        }

        /// <summary>
        /// Make sure the ListView has the extended style that says to display subitem images.
        /// </summary>
        /// <remarks>This method must be called after any .NET call that update the extended styles
        /// since they seem to erase this setting.</remarks>
        protected void ForceSubItemImagesExStyle()
        {
            NativeMethods.ForceSubItemImagesExStyle(this);
        }

        /// <summary>
        /// Convert the given image selector to an index into our image list.
        /// Return -1 if that's not possible
        /// </summary>
        /// <param name="imageSelector"></param>
        /// <returns>Index of the image in the imageList, or -1</returns>
        public int GetActualImageIndex(Object imageSelector)
        {
            if (imageSelector == null)
                return -1;

            if (imageSelector is Int32)
                return (int)imageSelector;

            if (imageSelector is String && this.SmallImageList != null)
                return this.SmallImageList.Images.IndexOfKey((String)imageSelector);

            return -1;
        }

        /// <summary>
        /// Prepare the listview to show alternate row backcolors
        /// </summary>
        /// <remarks>We cannot rely on lvi.Index in this method.
        /// In a straight list, lvi.Index is the display index, and can be used to determine
        /// whether the row should be colored. But when organised by groups, lvi.Index is not
        /// useable because it still refers to the position in the overall list, not the display order.
        ///</remarks>
        virtual protected void PrepareAlternateBackColors()
        {
            Color rowBackColor = this.AlternateRowBackColorOrDefault;
            int i = 0;

            if (this.ShowGroups) {
                foreach (ListViewGroup group in this.Groups) {
                    foreach (ListViewItem lvi in group.Items) {
                        if (i % 2 == 0)
                            lvi.BackColor = this.BackColor;
                        else
                            lvi.BackColor = rowBackColor;
                        CorrectSubItemColors(lvi);
                        i++;
                    }
                }
            } else {
                foreach (ListViewItem lvi in this.Items) {
                    if (i % 2 == 0)
                        lvi.BackColor = this.BackColor;
                    else
                        lvi.BackColor = rowBackColor;
                    CorrectSubItemColors(lvi);
                    i++;
                }
            }
        }

        /// <summary>
        /// Setup all subitem images on all rows
        /// </summary>
        protected void SetAllSubItemImages()
        {
            if (!this.ShowImagesOnSubItems)
                return;

            this.ForceSubItemImagesExStyle();

            for (int rowIndex = 0; rowIndex < this.GetItemCount(); rowIndex++)
                SetSubItemImages(rowIndex, this.GetItem(rowIndex));
        }

        /// <summary>
        /// For the given item and subitem, make it display the given image
        /// </summary>
        /// <param name="itemIndex">row number (0 based)</param>
        /// <param name="subItemIndex">subitem (0 is the item itself)</param>
        /// <param name="imageIndex">index into the image list</param>
        protected void SetSubItemImage(int itemIndex, int subItemIndex, int imageIndex)
        {
            NativeMethods.SetSubItemImage(this, itemIndex, subItemIndex, imageIndex);
        }

        /// <summary>
        /// Tell the underlying list control which images to show against the subitems
        /// </summary>
        /// <param name="rowIndex">the index at which the item occurs</param>
        /// <param name="item">the item whose subitems are to be set</param>
        protected void SetSubItemImages(int rowIndex, OLVListItem item)
        {
            this.SetSubItemImages(rowIndex, item, false);
        }

        /// <summary>
        /// Tell the underlying list control which images to show against the subitems
        /// </summary>
        /// <param name="rowIndex">the index at which the item occurs</param>
        /// <param name="item">the item whose subitems are to be set</param>
        /// <param name="shouldClearImages">will existing images be cleared if no new image is provided?</param>
        protected void SetSubItemImages(int rowIndex, OLVListItem item, bool shouldClearImages)
        {
            if (!this.ShowImagesOnSubItems)
                return;

            for (int i = 1; i < item.SubItems.Count; i++) {
                int imageIndex = this.GetActualImageIndex(((OLVListSubItem)item.SubItems[i]).ImageSelector);
                if (shouldClearImages || imageIndex != -1)
                    this.SetSubItemImage(rowIndex, i, imageIndex);
            }
        }

        /// <summary>
        /// Take ownership of the 'objects' collection. This separats our collection from the source.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method
        /// separates the 'objects' instance variable from its source, so that any AddObject/RemoveObject
        /// calls will modify our collection and not the original colleciton.
        /// </para>
        /// <para>
        /// This method has the intentional side-effect of converting our list of objects to an ArrayList.
        /// </para>
        /// </remarks>
        virtual protected void TakeOwnershipOfObjects()
        {
            if (this.isOwnerOfObjects)
                return;

            this.isOwnerOfObjects = true;

            if (this.objects == null)
                this.objects = new ArrayList();
            else if (this.objects is ICollection)
                this.objects = new ArrayList((ICollection)this.objects);
            else {
                ArrayList newObjects = new ArrayList();
                foreach (object x in this.objects)
                    newObjects.Add(x);
                this.objects = newObjects;
            }
        }

        #endregion

        #region ISupportInitialize Members

        void ISupportInitialize.BeginInit()
        {
            this.Frozen = true;
        }

        void ISupportInitialize.EndInit()
        {
            this.Frozen = false;
        }

        #endregion

        #region Image list manipulation

        /// <summary>
        /// Update our externally visible image list so it holds the same images as our shadow list, but sized correctly
        /// </summary>
        private void SetupExternalImageList()
        {
            // If a row height hasn't been set, or an image list has been give which is the required size, just assign it
            if (rowHeight == -1 || (this.shadowedImageList != null && this.shadowedImageList.ImageSize.Height == rowHeight))
                base.SmallImageList = this.shadowedImageList;
            else
                base.SmallImageList = this.MakeResizedImageList(rowHeight, shadowedImageList);
        }

        /// <summary>
        /// Return a copy of the given source image list, where each image has been resized to be height x height in size.
        /// If source is null, an empty image list of the given size is returned
        /// </summary>
        /// <param name="height">Height and width of the new images</param>
        /// <param name="source">Source of the images (can be null)</param>
        /// <returns>A new image list</returns>
        private ImageList MakeResizedImageList(int height, ImageList source)
        {
            ImageList il = new ImageList();
            il.ImageSize = new Size(height, height);

            // If there's nothing to copy, just return the new list
            if (source == null)
                return il;

            il.TransparentColor = source.TransparentColor;
            il.ColorDepth = source.ColorDepth;

            // Fill the imagelist with resized copies from the source
            for (int i = 0; i < source.Images.Count; i++) {
                Bitmap bm = this.MakeResizedImage(height, source.Images[i], source.TransparentColor);
                il.Images.Add(bm);
            }

            // Give each image the same key it has in the original
            foreach (String key in source.Images.Keys) {
                il.Images.SetKeyName(source.Images.IndexOfKey(key), key);
            }

            return il;
        }

        /// <summary>
        /// Return a bitmap of the given height x height, which shows the given image, centred.
        /// </summary>
        /// <param name="height">Height and width of new bitmap</param>
        /// <param name="image">Image to be centred</param>
        /// <param name="transparent">The background color</param>
        /// <returns>A new bitmap</returns>
        private Bitmap MakeResizedImage(int height, Image image, Color transparent)
        {
            Bitmap bm = new Bitmap(height, height);
            Graphics g = Graphics.FromImage(bm);
            g.Clear(transparent);
            int x = Math.Max(0, (bm.Size.Width - image.Size.Width) / 2);
            int y = Math.Max(0, (bm.Size.Height - image.Size.Height) / 2);
            g.DrawImage(image, x, y, image.Size.Width, image.Size.Height);
            return bm;
        }
        
        /// <summary>
        /// Initialize the state image list with the required checkbox images
        /// </summary>
        protected void InitializeStateImageList()
        {
            if (this.StateImageList == null) {
                this.StateImageList = new ImageList();
                this.StateImageList.ImageSize = new Size(16, 16);
            }

            if (this.StateImageList.Images.Count == 0)
                this.AddCheckedImage(this.StateImageList, System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal);
            if (this.StateImageList.Images.Count <= 1)
                this.AddCheckedImage(this.StateImageList, System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal);
            if (this.StateImageList.Images.Count <= 2)
                this.AddCheckedImage(this.StateImageList, System.Windows.Forms.VisualStyles.CheckBoxState.MixedNormal);
        }

        private void AddCheckedImage(ImageList imageList, System.Windows.Forms.VisualStyles.CheckBoxState checkBoxState)
        {
            Bitmap bm = new Bitmap(imageList.ImageSize.Width, imageList.ImageSize.Height);
            Graphics g = Graphics.FromImage(bm);
            g.Clear(imageList.TransparentColor);
            CheckBoxRenderer.DrawCheckBox(g, new Point(0, 0), checkBoxState);
            imageList.Images.Add(bm);
        }

        #endregion

        #region Owner drawing

        /// <summary>
        /// Owner draw the column header
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
            base.OnDrawColumnHeader(e);
        }

        /// <summary>
        /// Owner draw the item
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            // If there is a custom renderer installed for the primary column,
            // and we're not in details view, give it a chance to draw the item.
            // So the renderer on the primary column can have two distinct tasks,
            // in details view, it draws the primary cell; in non-details view,
            // it draws the whole item.
            OLVColumn column = this.GetColumn(0);
            if (this.View != View.Details && column.RendererDelegate != null) {
                Object row = ((OLVListItem)e.Item).RowObject;
                e.DrawDefault = !column.RendererDelegate(e, e.Graphics, e.Bounds, row);
            } else
                e.DrawDefault = (this.View != View.Details);

            if (e.DrawDefault)
                base.OnDrawItem(e);
        }

        int[] columnRightEdge = new int[256]; // will anyone ever want more than 256 columns??

        /// <summary>
        /// Owner draw a single subitem
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {
            // Don't try to do owner drawing at design time
            if (this.DesignMode) {
                e.DrawDefault = true;
                return;
            }
            
            // Calculate where the subitem should be drawn
            // It should be as simple as 'e.Bounds', but it isn't :-(

            // There seems to be a bug in .NET where the left edge of the bounds of subitem 0
            // is always 0. This is normally what is required, but it's wrong when
            // someone drags column 0 to somewhere else in the listview. We could
            // drop down into Windows-ville and use LVM_GETSUBITEMRECT, but just to be different
            // we keep track of the right edge of all columns, and when subitem 0
            // isn't being shown at column 0, we make its left edge to be the right
            // edge of the previous column plus a little bit.
            // NOTE: I considered replacing this with LVM_GETSUBITEMRECT, but apparently that has exactly 
            // same erroneous behavior.
            Rectangle r = e.Bounds;
            if (e.ColumnIndex == 0 && e.Header.DisplayIndex != 0) {
                r.X = this.columnRightEdge[e.Header.DisplayIndex - 1] + 1;
            } else
                this.columnRightEdge[e.Header.DisplayIndex] = e.Bounds.Right;

            // Get the special renderer for this column. If there isn't one, use the default draw mechanism.
            OLVColumn column = this.GetColumn(e.ColumnIndex);
            if (column.RendererDelegate == null) {
                e.DrawDefault = true;
                return;
            }
#if !MONO
            // Optimize drawing by only redrawing subitems that touch the area that was damaged
            if (!r.IntersectsWith(this.lastUpdateRectangle)) {
                return;
            }
#endif
            // Get a graphics context for the renderer to use.
            // But we have more complications. Virtual lists have a nasty habit of drawing column 0
            // whenever there is any mouse move events over a row, and doing it in an un-double-buffered manner,
            // which results in nasty flickers! There are also some unbuffered draw when a mouse is first
            // hovered over column 0 of a normal row. So, to avoid all complications,
            // we always manually double-buffer the drawing.
            // Except with Mono, which doesn't seem to handle double buffering at all :-(
            Graphics g = e.Graphics;
            BufferedGraphics buffer = null;
#if !MONO
            bool avoidFlickerMode = true; // set this to false to see the problems with flicker
            if (avoidFlickerMode) {
                buffer = BufferedGraphicsManager.Current.Allocate(e.Graphics, r);
                g = buffer.Graphics;
            }
#endif
            // Finally, give the renderer a chance to draw something
            Object row = ((OLVListItem)e.Item).RowObject;
            e.DrawDefault = !column.RendererDelegate(e, g, r, row);

            if (!e.DrawDefault && buffer != null) {
                buffer.Render();
                buffer.Dispose();
            }
        }

        #endregion

        #region Selection Event Handling

        /// <summary>
        /// This method is called every time a row is selected or deselected. This can be
        /// a pain if the user shift-clicks 100 rows. We override this method so we can
        /// trigger one event for any number of select/deselects that come from one user action
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);

            // If we haven't already scheduled an event, schedule it to be triggered
            // By using idle time, we will wait until all select events for the same
            // user action have finished before triggering the event.
            if (!this.hasIdleHandler) {
                this.hasIdleHandler = true;
                Application.Idle += new EventHandler(Application_Idle);
            }
        }
        private bool hasIdleHandler = false;

        /// <summary>
        /// The application is idle. Trigger a SelectionChanged event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Idle(object sender, EventArgs e)
        {
            // Remove the handler before triggering the event
            Application.Idle -= new EventHandler(Application_Idle);
            this.hasIdleHandler = false;

            this.OnSelectionChanged(new EventArgs());
        }

        /// <summary>
        /// This event is triggered once per user action that changes the selection state
        /// of one or more rows.
        /// </summary>
        [Category("Behavior"),
        Description("This event is triggered once per user action that changes the selection state of one or more rows.")]
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Trigger the SelectionChanged event
        /// </summary>
        /// <param name="e"></param>
        virtual protected void OnSelectionChanged(EventArgs e)
        {
            if (this.SelectionChanged != null)
                this.SelectionChanged(this, e);
        }

        #endregion

        #region Cell editing

        // What event should we listen for?
        // --------------------------------
        //
        // We can't use OnMouseClick, OnMouseDoubleClick, OnClick, or OnDoubleClick
        // since they are not triggered for clicks on subitems without Full Row Select.
        //
        // We could use OnMouseDown, but selecting rows is done in OnMouseUp. This means
        // that if we start the editing during OnMouseDown, the editor will automatically
        // lose focus when mouse up happens.
        //

        /// <summary>
        /// We need the click count in the mouse up event, but that is always 1.
        /// So we have to remember the click count from the preceding mouse down event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.lastMouseDownClickCount = e.Clicks;
        }
        private int lastMouseDownClickCount = 0;

        /// <summary>
        /// Check to see if we need to start editing a cell
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            // If it was an unmodified left click, check to see if we should start editing
            if (this.ShouldStartCellEdit(e)) {
                ListViewHitTestInfo info = this.HitTest(e.Location);
                if (info.Item != null && info.SubItem != null) {
                    int subItemIndex = info.Item.SubItems.IndexOf(info.SubItem);

                    // We don't edit the primary column by single clicks -- only subitems.
                    if (this.CellEditActivation != CellEditActivateMode.SingleClick || subItemIndex > 0)
                        this.EditSubItem((OLVListItem)info.Item, subItemIndex);
                }
            }
        }

        /// <summary>
        /// Should we start editing the cell?
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected bool ShouldStartCellEdit(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return false;

            if ((Control.ModifierKeys & (Keys.Shift | Keys.Control | Keys.Alt)) != 0)
                return false;

            if (this.lastMouseDownClickCount == 1 && this.CellEditActivation == CellEditActivateMode.SingleClick)
                return true;

            return (this.lastMouseDownClickCount == 2 && this.CellEditActivation == CellEditActivateMode.DoubleClick);
        }

        /// <summary>
        /// Handle a key press on this control. We specifically look for F2 which edits the primary column,
        /// or a Tab character during an edit operation, which tries to start editing on the next (or previous) cell.
        /// </summary>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            bool isSimpleTabKey = ((keyData & Keys.KeyCode) == Keys.Tab) && ((keyData & (Keys.Alt | Keys.Control)) == Keys.None);

            if (isSimpleTabKey && this.IsCellEditing) { // Tab key while editing
                // If the cell editing was cancelled, don't handle the tab
                if (!this.PossibleFinishCellEditing())
                    return true;

                // We can only Tab between columns when we are in Details view
                if (this.View != View.Details)
                    return true;

                OLVListItem olvi = this.cellEditEventArgs.ListViewItem;
                int subItemIndex = this.cellEditEventArgs.SubItemIndex;
                int displayIndex = this.GetColumn(subItemIndex).DisplayIndex;

                // Move to the next or previous editable subitem, depending on Shift key. Wrap at the edges
                List<OLVColumn> columnsInDisplayOrder = this.ColumnsInDisplayOrder;
                do {
                    if ((keyData & Keys.Shift) == Keys.Shift)
                        displayIndex = (olvi.SubItems.Count + displayIndex - 1) % olvi.SubItems.Count;
                    else
                        displayIndex = (displayIndex + 1) % olvi.SubItems.Count;
                } while (!columnsInDisplayOrder[displayIndex].IsEditable);

                // If we found a different editable cell, start editing it
                subItemIndex = columnsInDisplayOrder[displayIndex].Index;
                if (this.cellEditEventArgs.SubItemIndex != subItemIndex) {
                    this.StartCellEdit(olvi, subItemIndex);
                    return true;
                }
            }

            // Treat F2 as a request to edit the primary column
            if (keyData == Keys.F2 && !this.IsCellEditing) {
                this.EditSubItem((OLVListItem)this.FocusedItem, 0);
                return true;
            }

            // We have to catch Return/Enter/Escape here since some types of controls
            // (e.g. ComboBox, UserControl) don't trigger key events that we can listen for.
            // Treat Return or Enter as committing the current edit operation
            if ((keyData == Keys.Return || keyData == Keys.Enter) && this.IsCellEditing) {
                this.PossibleFinishCellEditing();
                return true;
            }

            // Treat Escaoe as cancel the current edit operation
            if (keyData == Keys.Escape && this.IsCellEditing) {
                this.CancelCellEdit();
                return true;
            }

            // Treat Ctrl-C as Copy To Clipboard. We still allow the default processing
            if ((keyData & Keys.Control) == Keys.Control && (keyData & Keys.KeyCode) == Keys.C)
                this.CopySelectionToClipboard();

            return base.ProcessDialogKey(keyData);
        }

        /// <summary>
        /// Begin an edit operation on the given cell.
        /// </summary>
        /// <remarks>This performs various sanity checks and passes off the real work to StartCellEdit().</remarks>
        /// <param name="item">The row to be edited</param>
        /// <param name="subItemIndex">The index of the cell to be edited</param>
        public void EditSubItem(OLVListItem item, int subItemIndex)
        {
            if (item == null)
                return;

            if (subItemIndex < 0 && subItemIndex >= item.SubItems.Count)
                return;

            if (this.CellEditActivation == CellEditActivateMode.None)
                return;

            if (!this.GetColumn(subItemIndex).IsEditable)
                return;

            this.StartCellEdit(item, subItemIndex);
        }

        /// <summary>
        /// Really start an edit operation on a given cell. The parameters are assumed to be sane.
        /// </summary>
        /// <param name="item">The row to be edited</param>
        /// <param name="subItemIndex">The index of the cell to be edited</param>
        protected void StartCellEdit(OLVListItem item, int subItemIndex)
        {
            OLVColumn column = this.GetColumn(subItemIndex);
            Rectangle r = this.CalculateCellBounds(item, subItemIndex);
            Control c = this.GetCellEditor(item, subItemIndex);
            c.Bounds = r;

            // Try to align the control as the column is aligned. Not all controls support this property
            PropertyInfo pinfo = c.GetType().GetProperty("TextAlign");
            if (pinfo != null)
                pinfo.SetValue(c, column.TextAlign, null);

            // Give the control the value from the model
            this.SetControlValue(c, column.GetValue(item.RowObject), column.GetStringValue(item.RowObject));

            // Give the outside world the chance to munge with the process
            this.cellEditEventArgs = new CellEditEventArgs(column, c, r, item, subItemIndex);
            this.OnCellEditStarting(this.cellEditEventArgs);
            if (this.cellEditEventArgs.Cancel)
                return;

            // The event handler may have completely changed the control, so we need to remember it
            this.cellEditor = this.cellEditEventArgs.Control;

            // If the control isn't the height of the cell, centre it vertically. We don't
            // need to do this when in Tile view.
            if (this.View != View.Tile && this.cellEditor.Height != r.Height)
                this.cellEditor.Top += (r.Height - this.cellEditor.Height) / 2;

            this.Controls.Add(this.cellEditor);
            this.ConfigureControl();
            this.PauseAnimations(true);
        }
        private Control cellEditor = null;
        private CellEditEventArgs cellEditEventArgs = null;

        /// <summary>
        /// Try to give the given value to the provided control. Fall back to assigning a string
        /// if the value assignment fails.
        /// </summary>
        /// <param name="c">A control</param>
        /// <param name="value">The value to be given to the control</param>
        /// <param name="stringValue">The string to be given if the value doesn't work</param>
        protected void SetControlValue(Control c, Object value, String stringValue)
        {
            // Look for a property called "Value". We have to look twice, the first time might get an ambiguous
            PropertyInfo pinfo = null;
            try {
                pinfo = c.GetType().GetProperty("Value");
            } catch (AmbiguousMatchException) {
                // The lowest level class of the control must have overridden the "Value" property.
                // We now have to specifically  look for only public instance properties declared in the lowest level class.
                pinfo = c.GetType().GetProperty("Value", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
            }

            // If we found it, use it to assign a value, otherwise simply set the text
            if (pinfo != null) {
                try {
                    pinfo.SetValue(c, value, null);
                    return;
                } catch (TargetInvocationException) {
                    // Not a lot we can do about this one. Something went wrong in the bowels
                    // of the method. Let's take the ostrich approach and just ignore it :-)
                } catch (ArgumentException) {
                }
            }

            // There wasn't a Value property, or we couldn't set it, so set the text instead
            try {
                if (value is String)
                    c.Text = (String)value;
                else
                    c.Text = stringValue;
            } catch (ArgumentOutOfRangeException) {
                // The value couldn't be set via the Text property.
            }
        }

        /// <summary>
        /// Setup the given control to be a cell editor
        /// </summary>
        protected void ConfigureControl()
        {
            this.cellEditor.Validating += new CancelEventHandler(CellEditor_Validating);
            this.cellEditor.Select();
        }

        /// <summary>
        /// Return the value that the given control is showing
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected Object GetControlValue(Control c)
        {
            try {
                return c.GetType().InvokeMember("Value", BindingFlags.GetProperty, null, c, null);
            } catch (MissingMethodException) { // Microsoft throws this
                return c.Text;
            } catch (MissingFieldException) { // Mono throws this
                return c.Text;
            }
        }

        /// <summary>
        /// Called when the cell editor could be about to lose focus. Time to commit the change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CellEditor_Validating(object sender, CancelEventArgs e)
        {
            this.cellEditEventArgs.Cancel = false;
            this.OnCellEditorValidating(this.cellEditEventArgs);

            if (this.cellEditEventArgs.Cancel) {
                this.cellEditEventArgs.Control.Select();
                e.Cancel = true;
            } else
                FinishCellEdit();
        }

        /// <summary>
        /// Return the bounds of the given cell
        /// </summary>
        /// <param name="item">The row to be edited</param>
        /// <param name="subItemIndex">The index of the cell to be edited</param>
        /// <returns>A Rectangle</returns>
        protected Rectangle CalculateCellBounds(OLVListItem item, int subItemIndex)
        {
            // Item 0 is special. Its bounds include all subitems. To get just the bounds
            // of cell for item 0, we have to use GetItemRect().
            if (subItemIndex == 0) {
                return this.GetItemRect(item.Index, ItemBoundsPortion.Label);
            } else
                return item.SubItems[subItemIndex].Bounds;
        }

        /// <summary>
        /// Return a control that can be used to edit the value of the given cell.
        /// </summary>
        /// <param name="item">The row to be edited</param>
        /// <param name="subItemIndex">The index of the cell to be edited</param>
        /// <returns></returns>
        protected Control GetCellEditor(OLVListItem item, int subItemIndex)
        {
            OLVColumn column = this.GetColumn(subItemIndex);
            Object value = column.GetValue(item.RowObject);

            //THINK: Do we want to use a Registry instead of this cascade?
            if (value is Boolean)
                return new BooleanCellEditor();

            if (value is DateTime) {
                DateTimePicker c = new DateTimePicker();
                c.Format = DateTimePickerFormat.Short;
                return c;
            }
            if (value is Int32 || value is Int16 || value is Int64)
                return new IntUpDown();

            if (value is UInt16 || value is UInt32 || value is UInt64)
                return new UintUpDown();

            if (value is Single || value is Double)
                return new FloatCellEditor();

            return this.MakeDefaultCellEditor(column);
        }

        /// <summary>
        /// Return a TextBox that can be used as a default cell editor.
        /// </summary>
        /// <param name="column">What column does the cell belong to?</param>
        /// <returns></returns>
        private Control MakeDefaultCellEditor(OLVColumn column)
        {
            TextBox tb = new TextBox();
            String str;

            // Build a list of unique values, to be used as autocomplete on the editor
            Dictionary<String, bool> alreadySeen = new Dictionary<string, bool>();
            for (int i = 0; i < Math.Min(this.GetItemCount(), 1000); i++) {
                Object value = column.GetValue(this.GetModelObject(i));
                if (value is String)
                    str = (String)value;
                else
                    str = column.ValueToString(value);
                if (!alreadySeen.ContainsKey(str)) {
                    tb.AutoCompleteCustomSource.Add(str);
                    alreadySeen[str] = true;
                }
            }

            tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
            tb.AutoCompleteMode = AutoCompleteMode.Append;

            return tb;
        }

        /// <summary>
        /// Stop editing a cell and throw away any changes.
        /// </summary>
        protected void CancelCellEdit()
        {
            // Let the world know that the user has cancelled the edit operation
            this.cellEditEventArgs.Cancel = true;
            this.OnCellEditFinishing(this.cellEditEventArgs);

            // Now cleanup the editing process
            this.CleanupCellEdit();
        }

        /// <summary>
        /// If a cell edit is in progress, finish the edit
        /// </summary>
        /// <returns>Returns false if the finishing process was cancelled
        /// (i.e. the cell editor is still on screen)</returns>
        protected bool PossibleFinishCellEditing()
        {
            if (this.IsCellEditing) {
                this.cellEditEventArgs.Cancel = false;
                this.OnCellEditorValidating(this.cellEditEventArgs);

                if (this.cellEditEventArgs.Cancel)
                    return false;

                FinishCellEdit();
            }

            return true;
        }

        /// <summary>
        /// Finish the cell edit operation, writing changed data back to the model object
        /// </summary>
        protected void FinishCellEdit()
        {
            this.cellEditEventArgs.Cancel = false;
            this.OnCellEditFinishing(this.cellEditEventArgs);

            // If someone doesn't cancel the editing process, write the value back into the model
            if (!this.cellEditEventArgs.Cancel) {
                Object value = this.GetControlValue(this.cellEditor);
                this.cellEditEventArgs.Column.PutValue(this.cellEditEventArgs.RowObject, value);
                this.RefreshItem(this.cellEditEventArgs.ListViewItem);
            }

            this.CleanupCellEdit();
        }

        /// <summary>
        /// Remove all trace of any existing cell edit operation
        /// </summary>
        protected void CleanupCellEdit()
        {
            if (this.cellEditor == null)
                return;

            this.cellEditor.Validating -= new CancelEventHandler(CellEditor_Validating);
            this.Controls.Remove(this.cellEditor);
            this.cellEditor.Dispose(); //THINK: do we need to call this?
            this.cellEditor = null;
            this.Select();
            this.PauseAnimations(false);
        }

        /// <summary>
        /// Return a collection of columns that are appropriate to the given view.
        /// Only Tile and Details have columns; all other views have 0 columns.
        /// </summary>
        /// <param name="view">Which view are the columns being calculate for?</param>
        /// <returns>A list of columns</returns>
        public List<OLVColumn> GetFilteredColumns(View view)
        {
            // For both detail and tile view, the first column must be included. Normally, we would
            // use the ColumnHeader.Index property, but if the header is not currently part of a ListView
            // that property returns -1. So, we track the index of
            // the column header, and always include the first header.

            int index = 0;
            switch (view) {
                case View.Details:
                    return this.AllColumns.FindAll(delegate(OLVColumn x) { return (index++ == 0) || x.IsVisible; });
                case View.Tile:
                    return this.AllColumns.FindAll(delegate(OLVColumn x) { return (index++ == 0) || x.IsTileViewColumn; });
                default:
                    return new List<OLVColumn>(); ;
            }
        }

        #endregion

        #region Design Time

        /// <summary>
        /// This class works in conjunction with the OLVColumns property to allow OLVColumns
        /// to be added to the ObjectListView.
        /// </summary>
        internal class OLVColumnCollectionEditor : System.ComponentModel.Design.CollectionEditor
        {
            public OLVColumnCollectionEditor(Type t)
                : base(t)
            {
            }

            protected override Type CreateCollectionItemType()
            {
                return typeof(OLVColumn);
            }

            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                ListView.ColumnHeaderCollection cols = (ListView.ColumnHeaderCollection)value;

                // Hack to figure out which ObjectListView we are working on
                ObjectListView olv;
                if (cols.Count == 0) {
                    cols.Add(new OLVColumn());
                    olv = (ObjectListView)cols[0].ListView;
                    cols.Clear();
                    olv.AllColumns.Clear();
                } else
                    olv = (ObjectListView)cols[0].ListView;

                // Edit all the columns, not just the ones that are visible
                base.EditValue(context, provider, olv.AllColumns);

                // Calculate just the visible columns
                List<OLVColumn> newColumns = olv.GetFilteredColumns(View.Details);
                olv.Columns.Clear();
                olv.Columns.AddRange(newColumns.ToArray());

                return olv.Columns;
            }
        }

        /// <summary>
        /// Return Columns for this list. We hide the original so we can associate
        /// a specialised editor with it.
        /// </summary>
        [Editor(typeof(OLVColumnCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        new public ListView.ColumnHeaderCollection Columns
        {
            get
            {
                return base.Columns;
            }
        }

        #endregion

        private Rectangle lastUpdateRectangle; // remember the update rect from the last WM_PAINT msg
        private bool isOwnerOfObjects; // does this ObjectListView own the Objects collection?
    }

    #region Delegate declarations

    /// <summary>
    /// These delegates are used to extract an aspect from a row object
    /// </summary>
    public delegate Object AspectGetterDelegate(Object rowObject);


    /// <summary>
    /// These delegates are used to put a changed value back into a model object
    /// </summary>
    public delegate void AspectPutterDelegate(Object rowObject, Object newValue);

    /// <summary>
    /// These delegates can be used to convert an aspect value to a display string,
    /// instead of using the default ToString()
    /// </summary>
    public delegate string AspectToStringConverterDelegate(Object value);

    /// <summary>
    /// These delegates are used to the state of the checkbox for a row object.
    /// </summary>
    /// <remarks><para>
    /// For reasons known only to someone in Microsoft, we can only set
    /// a boolean on the ListViewItem to indicate it's "checked-ness", but when
    /// we receive update events, we have to use a tristate CheckState. So we can
    /// be told about an indeterminate state, but we can't set it ourselves.
    /// </para>
    /// <para>As of version 2.0, returning null is understood as indeterminate.</para>
    /// </remarks>
    public delegate bool? CheckStateGetterDelegate(Object rowObject);

    /// <summary>
    /// These delegates are used to put a changed check state back into a model object
    /// </summary>
    public delegate void CheckStatePutterDelegate(Object rowObject, CheckState newValue);

    /// <summary>
    /// These delegates are used to retrieve the object that is the key of the group to which the given row belongs.
    /// </summary>
    public delegate Object GroupKeyGetterDelegate(Object rowObject);

    /// <summary>
    /// These delegates are used to convert a group key into a title for the group
    /// </summary>
    public delegate string GroupKeyToTitleConverterDelegate(Object groupKey);

    /// <summary>
    /// These delegates are used to fetch the image selector that should be used
    /// to choose an image for this column.
    /// </summary>
    public delegate Object ImageGetterDelegate(Object rowObject);

    /// <summary>
    /// These delegates are used to draw a cell
    /// </summary>
    public delegate bool RenderDelegate(EventArgs e, Graphics g, Rectangle r, Object rowObject);

    /// <summary>
    /// These delegates are used to fetch a row object for virtual lists
    /// </summary>
    public delegate Object RowGetterDelegate(int rowIndex);

    /// <summary>
    /// These delegates are used to format a listviewitem before it is added to the control.
    /// </summary>
    public delegate void RowFormatterDelegate(OLVListItem olvItem);

    /// <summary>
    /// These delegates are used to sort the listview in some custom fashion
    /// </summary>
    public delegate void SortDelegate(OLVColumn column, SortOrder sortOrder);

    #endregion

    #region Column

    /// <summary>
    /// An OLVColumn knows which aspect of an object it should present.
    /// </summary>
    /// <remarks>
    /// The column knows how to:
    /// <list type="bullet">
    ///	<item>extract its aspect from the row object</item>
    ///	<item>convert an aspect to a string</item>
    ///	<item>calculate the image for the row object</item>
    ///	<item>extract a group "key" from the row object</item>
    ///	<item>convert a group "key" into a title for the group</item>
    /// </list>
    /// <para>For sorting to work correctly, aspects from the same column
    /// must be of the same type, that is, the same aspect cannot sometimes
    /// return strings and other times integers.</para>
    /// </remarks>
    [Browsable(false)]
    public partial class OLVColumn : ColumnHeader
    {
        /// <summary>
        /// Create an OLVColumn
        /// </summary>
        public OLVColumn()
            : base()
        {
        }

        /// <summary>
        /// Initialize a column to have the given title, and show the given aspect
        /// </summary>
        /// <param name="title">The title of the column</param>
        /// <param name="aspect">The aspect to be shown in the column</param>
        public OLVColumn(string title, string aspect)
            : this()
        {
            this.Text = title;
            this.AspectName = aspect;
        }

        #region Public Properties

        /// <summary>
        /// The name of the property or method that should be called to get the value to display in this column.
        /// This is only used if a ValueGetterDelegate has not been given.
        /// </summary>
        /// <remarks>This name can be dotted to chain references to properties or methods.</remarks>
        /// <example>"DateOfBirth"</example>
        /// <example>"Owner.HomeAddress.Postcode"</example>
        [Category("Behavior"),
         Description("The name of the property or method that should be called to get the aspect to display in this column")]
        public string AspectName
        {
            get { return aspectName; }
            set { aspectName = value; }
        }
        private string aspectName;

        /// <summary>
        /// This format string will be used to convert an aspect to its string representation.
        /// </summary>
        /// <remarks>
        /// This string is passed as the first parameter to the String.Format() method.
        /// This is only used if ToStringDelegate has not been set.</remarks>
        /// <example>"{0:C}" to convert a number to currency</example>
        [Category("Behavior"),
         Description("The format string that will be used to convert an aspect to its string representation"),
         DefaultValue(null)]
        public string AspectToStringFormat
        {
            get { return aspectToStringFormat; }
            set { aspectToStringFormat = value; }
        }
        private string aspectToStringFormat;

        /// <summary>
        /// Group objects by the initial letter of the aspect of the column
        /// </summary>
        /// <remarks>
        /// One common pattern is to group column by the initial letter of the value for that group.
        /// The aspect must be a string (obviously).
        /// </remarks>
        [Category("Behavior"),
         Description("The name of the property or method that should be called to get the aspect to display in this column"),
         DefaultValue(false)]
        public bool UseInitialLetterForGroup
        {
            get { return useInitialLetterForGroup; }
            set { useInitialLetterForGroup = value; }
        }
        private bool useInitialLetterForGroup;

        /// <summary>
        /// Get/set whether this column should be used when the view is switched to tile view.
        /// </summary>
        /// <remarks>Column 0 is always included in tileview regardless of this setting.
        /// Tile views do not work well with many "columns" of information, 2 or 3 works best.</remarks>
        [Category("Behavior"),
        Description("Will this column be used when the view is switched to tile view"),
         DefaultValue(false)]
        public bool IsTileViewColumn
        {
            get { return isTileViewColumn; }
            set { isTileViewColumn = value; }
        }
        private bool isTileViewColumn = false;

        /// <summary>
        /// This delegate will be used to extract a value to be displayed in this column.
        /// </summary>
        /// <remarks>
        /// If this is set, AspectName is ignored.
        /// </remarks>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AspectGetterDelegate AspectGetter
        {
            get { return aspectGetter; }
            set
            {
                aspectGetter = value;
                aspectGetterAutoGenerated = false;
            }
        }
        private AspectGetterDelegate aspectGetter;


        /// <summary>
        /// The delegate that will be used to translate the aspect to display in this column into a string.
        /// </summary>
        /// <remarks>If this value is set, ValueToStringFormat will be ignored.</remarks>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AspectToStringConverterDelegate AspectToStringConverter
        {
            get { return aspectToStringConverter; }
            set { aspectToStringConverter = value; }
        }
        private AspectToStringConverterDelegate aspectToStringConverter;

        /// <summary>
        /// This delegate is called to get the image selector of the image that should be shown in this column.
        /// It can return an int, string, Image or null.
        /// </summary>
        /// <remarks><para>This delegate can use these return value to identify the image:</para>
        /// <list>
        /// <item>null or -1 -- indicates no image</item>
        /// <item>an int -- the int value will be used as an index into the image list</item>
        /// <item>a String -- the string value will be used as a key into the image list</item>
        /// <item>an Image -- the Image will be drawn directly (only in OwnerDrawn mode)</item>
        /// </list>
        /// </remarks>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ImageGetterDelegate ImageGetter
        {
            get { return imageGetter; }
            set { imageGetter = value; }
        }
        private ImageGetterDelegate imageGetter;

        /// <summary>
        /// This delegate is called to get the object that is the key for the group
        /// to which the given row belongs.
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GroupKeyGetterDelegate GroupKeyGetter
        {
            get { return groupKeyGetter; }
            set { groupKeyGetter = value; }
        }
        private GroupKeyGetterDelegate groupKeyGetter;

        /// <summary>
        /// This delegate is called to convert a group key into a title for that group.
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GroupKeyToTitleConverterDelegate GroupKeyToTitleConverter
        {
            get { return groupKeyToTitleConverter; }
            set { groupKeyToTitleConverter = value; }
        }
        private GroupKeyToTitleConverterDelegate groupKeyToTitleConverter;

        /// <summary>
        /// This delegate is called when a cell needs to be drawn in OwnerDrawn mode.
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RenderDelegate RendererDelegate
        {
            get { return rendererDelegate; }
            set { rendererDelegate = value; }
        }
        private RenderDelegate rendererDelegate;

        /// <summary>
        /// Get/set the renderer that will be invoked when a cell needs to be redrawn
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BaseRenderer Renderer
        {
            get { return renderer; }
            set
            {
                renderer = value;
                if (renderer == null)
                    this.RendererDelegate = null;
                else {
                    renderer.Column = this;
                    this.RendererDelegate = new RenderDelegate(renderer.HandleRendering);
                }
            }
        }
        private BaseRenderer renderer;

        /// <summary>
        /// Remember if this aspect getter for this column was generated internally, and can therefore
        /// be regenerated at will
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AspectGetterAutoGenerated
        {
            get { return aspectGetterAutoGenerated; }
            set { aspectGetterAutoGenerated = value; }
        }
        private bool aspectGetterAutoGenerated;

        /// <summary>
        /// When the listview is grouped by this column and group title has an item count,
        /// how should the lable be formatted?
        /// </summary>
        /// <remarks>
        /// The given format string can/should have two placeholders:
        /// <list type="bullet">
        /// <item>{0} - the original group title</item>
        /// <item>{1} - the number of items in the group</item>
        /// </list>
        /// <para>If this value is not set, the values from the list view will be used</para>
        /// </remarks>
        /// <example>"{0} [{1} items]"</example>
        [Category("Behavior"),
         Description("The format to use when suffixing item counts to group titles"),
         DefaultValue(null)]
        public string GroupWithItemCountFormat
        {
            get { return groupWithItemCountFormat; }
            set { groupWithItemCountFormat = value; }
        }
        private string groupWithItemCountFormat;

        /// <summary>
        /// Return this.GroupWithItemCountFormat or a reasonable default
        /// </summary>
        [Browsable(false)]
        public string GroupWithItemCountFormatOrDefault
        {
            get
            {
                if (String.IsNullOrEmpty(this.GroupWithItemCountFormat))
                    // There is one rare but pathelogically possible case where the ListView can
                    // be null, so we have to provide a workable default for that rare case.
                    if (this.ListView == null)
                        return "{0} [{1} items]";
                    else
                        return ((ObjectListView)this.ListView).GroupWithItemCountFormatOrDefault;
                else
                    return this.GroupWithItemCountFormat;
            }
        }

        /// <summary>
        /// When the listview is grouped by this column and a group title has an item count,
        /// how should the lable be formatted if there is only one item in the group?
        /// </summary>
        /// <remarks>
        /// The given format string can/should have two placeholders:
        /// <list type="bullet">
        /// <item>{0} - the original group title</item>
        /// <item>{1} - the number of items in the group (always 1)</item>
        /// </list>
        /// <para>If this value is not set, the values from the list view will be used</para>
        /// </remarks>
        /// <example>"{0} [{1} item]"</example>
        [Category("Behavior"),
         Description("The format to use when suffixing item counts to group titles"),
         DefaultValue(null)]
        public string GroupWithItemCountSingularFormat
        {
            get { return groupWithItemCountSingularFormat; }
            set { groupWithItemCountSingularFormat = value; }
        }
        private string groupWithItemCountSingularFormat;

        /// <summary>
        /// Return this.GroupWithItemCountSingularFormat or a reasonable default
        /// </summary>
        [Browsable(false)]
        public string GroupWithItemCountSingularFormatOrDefault
        {
            get
            {
                if (String.IsNullOrEmpty(this.GroupWithItemCountSingularFormat))
                    // There is one pathelogically rare but still possible case where the ListView can
                    // be null, so we have to provide a workable default for that rare case.
                    if (this.ListView == null)
                        return "{0} [{1} item]";
                    else
                        return ((ObjectListView)this.ListView).GroupWithItemCountSingularFormatOrDefault;
                else
                    return this.GroupWithItemCountSingularFormat;
            }
        }

        /// <summary>
        /// What is the minimum width that the user can give to this column?
        /// </summary>
        /// <remarks>-1 means there is no minimum width. Give this the same value as MaximumWidth to make a fixed width column.</remarks>
        [Category("Misc"),
         Description("What is the minimum width to which the user can resize this column?"),
         DefaultValue(-1)]
        public int MinimumWidth
        {
            get { return minWidth; }
            set
            {
                minWidth = value;
                if (this.Width < minWidth)
                    this.Width = minWidth;
            }
        }
        private int minWidth = -1;

        /// <summary>
        /// What is the maximum width that the user can give to this column?
        /// </summary>
        /// <remarks>-1 means there is no maximum width. Give this the same value as MinimumWidth to make a fixed width column.</remarks>
        [Category("Misc"),
         Description("What is the maximum width to which the user can resize this column?"),
         DefaultValue(-1)]
        public int MaximumWidth
        {
            get { return maxWidth; }
            set
            {
                maxWidth = value;
                if (maxWidth != -1 && this.Width > maxWidth)
                    this.Width = maxWidth;
            }
        }
        private int maxWidth = -1;

        /// <summary>
        /// Is this column a fixed width column?
        /// </summary>
        [Browsable(false)]
        public bool IsFixedWidth
        {
            get
            {
                return (this.MinimumWidth != -1 && this.MaximumWidth != -1 && this.MinimumWidth >= this.MaximumWidth);
            }
        }

        /// <summary>
        /// What proportion of the unoccupied horizontal space in the control should be given to this column?
        /// </summary>
        /// <remarks>
        /// <para>
        /// There are situations where it would be nice if a column (normally the rightmost one) would expand as
        /// the list view expands, so that as much of the column was visible as possible without having to scroll
        /// horizontally (you should never, ever make your users have to scroll anything horizontally!).
        /// </para>
        /// <para>
        /// A space filling column is resized to occupy a proportion of the unoccupied width of the listview (the
        /// unoccupied width is the width left over once all the the non-filling columns have been given their space).
        /// This property indicates the relative proportion of that unoccupied space that will be given to this column.
        /// The actual value of this property is not important -- only its value relative to the value in other columns.
        /// For example:
        /// <list type="bullet">
        /// <item>
        /// If there is only one space filling column, it will be given all the free space, regardless of the value in FreeSpaceProportion.
        /// </item>
        /// <item>
        /// If there are two or more space filling columns and they all have the same value for FreeSpaceProportion,
        /// they will share the free space equally.
        /// </item>
        /// <item>
        /// If there are three space filling columns with values of 3, 2, and 1
        /// for FreeSpaceProportion, then the first column with occupy half the free space, the second will
        /// occupy one-third of the free space, and the third column one-sixth of the free space.
        /// </item>
        /// </list>
        /// </para>
        /// </remarks>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int FreeSpaceProportion
        {
            get { return freeSpaceProportion; }
            set { freeSpaceProportion = Math.Max(0, value); }
        }
        private int freeSpaceProportion = 0;

        /// <summary>
        /// Should this column resize to fill the free space in the listview?
        /// </summary>
        /// <remarks>
        /// <para>
        /// If you want two (or more) columns to equally share the available free space, set this property to True.
        /// If you want this column to have a larger or smaller share of the free space, you must
        /// set the FreeSpaceProportion property explicitly.
        /// </para>
        /// <para>
        /// Space filling columns are still governed by the MinimumWidth and MaximumWidth properties.
        /// </para>
        /// /// </remarks>
        [Category("Misc"),
         Description("Will this column resize to fill unoccupied horizontal space in the listview?"),
         DefaultValue(false)]
        public bool FillsFreeSpace
        {
            get { return this.FreeSpaceProportion > 0; }
            set
            {
                if (value)
                    this.freeSpaceProportion = 1;
                else
                    this.freeSpaceProportion = 0;
            }
        }

        /// <summary>
        /// This delegate will be used to put an edited value back into the model object.
        /// </summary>
        /// <remarks>
        /// This does nothing if IsEditable == false.
        /// </remarks>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AspectPutterDelegate AspectPutter
        {
            get { return aspectPutter; }
            set { aspectPutter = value; }
        }
        private AspectPutterDelegate aspectPutter;

        /// <summary>
        /// Can the values shown in this column be edited?
        /// </summary>
        /// <remarks>This defaults to true, since the primary means to control the editability of a listview
        /// is on the listview itself. Once a listview is editable, all the columns are too, unless the
        /// programmer explicitly marks them as not editable</remarks>
        [Category("Misc"),
        Description("Can the value in this column be edited?"),
        DefaultValue(true)]
        public bool IsEditable
        {
            get { return isEditable; }
            set { isEditable = value; }
        }
        private bool isEditable = true;

        /// <summary>
        /// Return the control that should be used to edit cells in this column
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Control CellEditor
        {
            get { return cellEditor; }
            set { cellEditor = value; }
        }
        private Control cellEditor;

        /// <summary>
        /// Can this column be seen by the user?
        /// </summary>
        /// <remarks>After changing this value, you must call RebuildColumns() before the changes will be effected.</remarks>
        [Category("Misc"),
        Description("Can this column be seen by the user?"),
        DefaultValue(true)]
        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }
        private bool isVisible = true;

        /// <summary>
        /// Where was this column last positioned within the Detail view columns
        /// </summary>
        /// <remarks>DisplayIndex is volatile. Once a column is removed from the control,
        /// there is no way to discover where it was in the display order. This property
        /// guards that information even when the column is not in the listview's active columns.</remarks>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int LastDisplayIndex = -1;

        #endregion

        /// <summary>
        /// For a given row object, return the object that is to be displayed in this column.
        /// </summary>
        /// <param name="rowObject">The row object that is being displayed</param>
        /// <returns>An object, which is the aspect to be displayed</returns>
        public object GetValue(object rowObject)
        {
            if (this.aspectGetter == null)
                return this.GetAspectByName(rowObject);
            else
                return this.aspectGetter(rowObject);
        }

        /// <summary>
        /// For a given row object, extract the value indicated by the AspectName property of this column.
        /// </summary>
        /// <param name="rowObject">The row object that is being displayed</param>
        /// <returns>An object, which is the aspect named by AspectName</returns>
        public object GetAspectByName(object rowObject)
        {
            if (rowObject == null || string.IsNullOrEmpty(this.aspectName))
                return null;

            //CONSIDER: Should we include NonPublic in this list?
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.GetField;
            object source = rowObject;
            foreach (string property in this.aspectName.Split('.')) {
                try {
                    source = source.GetType().InvokeMember(property, flags, null, source, null);
                    if (source == null)
                        break;
                } catch (System.MissingMethodException) {
                    return String.Format("Cannot invoke '{0}' on a {1}", property, source.GetType());
                }
            }
            return source;
        }

        /// <summary>
        /// Update the given model object with the given value
        /// </summary>
        /// <param name="rowObject">The model object to be updated</param>
        /// <param name="newValue">The value to be put into the model</param>
        public void PutValue(Object rowObject, Object newValue)
        {
            if (this.aspectPutter == null)
                this.PutAspectByName(rowObject, newValue);
            else
                this.aspectPutter(rowObject, newValue);
        }

        /// <summary>
        /// Update the given model object with the given value using the column's
        /// AspectName.
        /// </summary>
        /// <param name="rowObject">The model object to be updated</param>
        /// <param name="newValue">The value to be put into the model</param>
        public void PutAspectByName(Object rowObject, Object newValue)
        {
            if (string.IsNullOrEmpty(this.aspectName))
                return;

            // Navigated through the dotted path until we reach the target object.
            // We then try to set the last property on the dotted path on that target.
            // So, if rowObject is a Person, then an aspect named "HomeAddress.Postcode"
            // will first fetch the "HomeAddress" property, and then try to set the
            // "Postcode" property on the home address object.

            //CONSIDER: Should we include NonPublic in this list?
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.GetField;
            Object target = rowObject;
            List<String> parentProperties = new List<string>(this.aspectName.Split('.'));
            String lastProperty = parentProperties[parentProperties.Count - 1];
            parentProperties.RemoveAt(parentProperties.Count - 1);
            foreach (string property in parentProperties) {
                try {
                    target = target.GetType().InvokeMember(property, flags, null, target, null);
                } catch (System.MissingMethodException) {
                    System.Diagnostics.Debug.WriteLine(String.Format("Cannot invoke '{0}' on a {1}", property, target.GetType()));
                    return;
                }
            }

            // Now try to set the value
            try {
                BindingFlags flags2 = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.SetField;
                target.GetType().InvokeMember(lastProperty, flags2, null, target, new Object[] { newValue });
            } catch (System.MissingMethodException ex) {
                System.Diagnostics.Debug.WriteLine("Invoke PutAspectByName failed:");
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        /// <summary>
        /// For a given row object, return the string representation of the value shown in this column.
        /// </summary>
        /// <remarks>
        /// For aspects that are string (e.g. aPerson.Name), the aspect and its string representation are the same.
        /// For non-strings (e.g. aPerson.DateOfBirth), the string representation is very different.
        /// </remarks>
        /// <param name="rowObject"></param>
        /// <returns></returns>
        public string GetStringValue(object rowObject)
        {
            return this.ValueToString(this.GetValue(rowObject));
        }

        /// <summary>
        /// Convert the aspect object to its string representation.
        /// </summary>
        /// <remarks>
        /// If the column has been given a ToStringDelegate, that will be used to do
        /// the conversion, otherwise just use ToString(). Nulls are always converted
        /// to empty strings.
        /// </remarks>
        /// <param name="value">The value of the aspect that should be displayed</param>
        /// <returns>A string representation of the aspect</returns>
        public string ValueToString(object value)
        {
            // CONSIDER: Should we give aspect-to-string converters a chance to work on a null value?
            if (value == null)
                return "";

            if (this.aspectToStringConverter != null)
                return this.aspectToStringConverter(value);

            string fmt = this.AspectToStringFormat;
            if (String.IsNullOrEmpty(fmt))
                return value.ToString();
            else
                return String.Format(fmt, value);
        }

        /// <summary>
        /// For a given row object, return the image selector of the image that should displayed in this column.
        /// </summary>
        /// <param name="rowObject">The row object that is being displayed</param>
        /// <returns>int or string or Image. int or string will be used as index into image list. null or -1 means no image</returns>
        public Object GetImage(object rowObject)
        {
            if (this.imageGetter != null)
                return this.imageGetter(rowObject);

            if (!String.IsNullOrEmpty(this.ImageKey))
                return this.ImageKey;

            return this.ImageIndex;
        }

        /// <summary>
        /// For a given row object, return the object that is the key of the group that this row belongs to.
        /// </summary>
        /// <param name="rowObject">The row object that is being displayed</param>
        /// <returns>Group key object</returns>
        public object GetGroupKey(object rowObject)
        {
            if (this.groupKeyGetter == null) {
                object key = this.GetValue(rowObject);
                if (key is string && this.UseInitialLetterForGroup) {
                    String keyAsString = (String)key;
                    if (keyAsString.Length > 0)
                        key = keyAsString.Substring(0, 1).ToUpper();
                }
                return key;
            } else
                return this.groupKeyGetter(rowObject);
        }

        /// <summary>
        /// For a given group value, return the string that should be used as the groups title.
        /// </summary>
        /// <param name="value">The group key that is being converted to a title</param>
        /// <returns>string</returns>
        public string ConvertGroupKeyToTitle(object value)
        {
            if (this.groupKeyToTitleConverter == null)
                return this.ValueToString(value);
            else
                return this.groupKeyToTitleConverter(value);
        }

        #region Utilities

        /// <summary>
        /// Install delegates that will group the columns aspects into progressive partitions.
        /// If an aspect is less than value[n], it will be grouped with description[n].
        /// If an aspect has a value greater than the last element in "values", it will be grouped
        /// with the last element in "descriptions".
        /// </summary>
        /// <param name="values">Array of values. Values must be able to be
        /// compared to the aspect (using IComparable)</param>
        /// <param name="descriptions">The description for the matching value. The last element is the default description.
        /// If there are n values, there must be n+1 descriptions.</param>
        /// <example>
        /// this.salaryColumn.MakeGroupies(
        ///     new UInt32[] { 20000, 100000 },
        ///     new string[] { "Lowly worker",  "Middle management", "Rarified elevation"});
        /// </example>
        public void MakeGroupies<T>(T[] values, string[] descriptions)
        {
            if (values.Length + 1 != descriptions.Length)
                throw new ArgumentException("descriptions must have one more element than values.");

            // Install a delegate that returns the index of the description to be shown
            this.GroupKeyGetter = delegate(object row) {
                Object aspect = this.GetValue(row);
                if (aspect == null || aspect == System.DBNull.Value)
                    return -1;
                IComparable comparable = (IComparable)aspect;
                for (int i = 0; i < values.Length; i++) {
                    if (comparable.CompareTo(values[i]) < 0)
                        return i;
                }

                // Display the last element in the array
                return descriptions.Length - 1;
            };

            // Install a delegate that simply looks up the given index in the descriptions.
            this.GroupKeyToTitleConverter = delegate(object key) {
                if ((int)key < 0)
                    return "";

                return descriptions[(int)key];
            };
        }

        #endregion
    }

    #endregion

    #region OLVListItem and OLVListSubItem

    /// <summary>
    /// OLVListItems are specialized ListViewItems that know which row object they came from,
    /// and the row index at which they are displayed, even when in group view mode. They
    /// also know the image they should draw against themselves
    /// </summary>
    public class OLVListItem : ListViewItem
    {
        /// <summary>
        /// Create a OLVListItem for the given row object
        /// </summary>
        public OLVListItem(object rowObject)
            : base()
        {
            this.rowObject = rowObject;
        }

        /// <summary>
        /// Create a OLVListItem for the given row object, represented by the given string and image
        /// </summary>
        public OLVListItem(object rowObject, string text, Object image)
            : base(text, -1)
        {
            this.rowObject = rowObject;
            this.imageSelector = image;
        }

        /// <summary>
        /// RowObject is the model object that is source of the data for this list item.
        /// </summary>
        public object RowObject
        {
            get { return rowObject; }
            set { rowObject = value; }
        }
        private object rowObject;

        /// <summary>
        /// DisplayIndex is the index of the row where this item is displayed. For flat lists,
        /// this is the same as ListViewItem.Index, but for grouped views, it is different.
        /// </summary>
        [Obsolete("This property is no longer maintained", true)]
        public int DisplayIndex
        {
            get { return 0; }
            set {  }
        }

        /// <summary>
        /// Get or set the image that should be shown against this item
        /// </summary>
        /// <remarks><para>This can be an Image, a string or an int. A string or an int will
        /// be used as an index into the small image list.</para></remarks>
        public Object ImageSelector
        {
            get { return imageSelector; }
            set
            {
                imageSelector = value;
                if (value is Int32)
                    this.ImageIndex = (Int32)value;
                else if (value is String)
                    this.ImageKey = (String)value;
                else
                    this.ImageIndex = -1;
            }
        }
        private Object imageSelector;

        /// <summary>
        /// Enable tri-state checkbox.
        /// </summary>
        public CheckState CheckState
        {
            get { return this.checkState; }
            set { 
                this.checkState = value;
                this.Checked = (checkState == System.Windows.Forms.CheckState.Checked);

                // We have to specifically set the state image
                switch (value) {
                    case System.Windows.Forms.CheckState.Unchecked:
                        this.StateImageIndex = 0;
                        break;
                    case System.Windows.Forms.CheckState.Checked:
                        this.StateImageIndex = 1;
                        break;
                    case System.Windows.Forms.CheckState.Indeterminate:
                        this.StateImageIndex = 2;
                        break;
                }
            }
        }
        private CheckState checkState;

        /// <summary>
        /// Set the value of the check box.
        /// </summary>
        /// <param name="value">true, false or null, where null means undecided</param>
        public void SetCheckState(bool? value)
        {
            if (value.HasValue)
                if (value == true)
                    this.CheckState = System.Windows.Forms.CheckState.Checked;
                else
                    this.CheckState = System.Windows.Forms.CheckState.Unchecked;
            else
                this.CheckState = System.Windows.Forms.CheckState.Indeterminate;
        }
    }

    /// <summary>
    /// A ListViewSubItem that knows which image should be drawn against it.
    /// </summary>
    [Browsable(false)]
    public class OLVListSubItem : ListViewItem.ListViewSubItem
    {
        /// <summary>
        /// Create a OLVListSubItem
        /// </summary>
        public OLVListSubItem()
            : base()
        {
        }

        /// <summary>
        /// Create a OLVListSubItem that shows the given string and image
        /// </summary>
        public OLVListSubItem(string text, Object image)
            : base()
        {
            this.Text = text;
            this.ImageSelector = image;
        }

        /// <summary>
        /// Get or set the image that should be shown against this item
        /// </summary>
        /// <remarks><para>This can be an Image, a string or an int. A string or an int will
        /// be used as an index into the small image list.</para></remarks>
        public Object ImageSelector
        {
            get { return imageSelector; }
            set { imageSelector = value; }
        }
        private Object imageSelector;


        /// <summary>
        /// Return the state of the animatation of the image on this subitem.
        /// Null means there is either no image, or it is not an animation
        /// </summary>
        internal ImageRenderer.AnimationState AnimationState
        {
            get { return animationState; }
            set { animationState = value; }
        }
        private ImageRenderer.AnimationState animationState;

    }

    #endregion

    #region Comparers

    /// <summary>
    /// This comparer sort list view groups.
    /// It does this on the basis of the values in the Tags, if we can figure out how to compare
    /// objects of that type. Failing that, it uses a case insensitive compare on the group header.
    /// </summary>
    internal class ListViewGroupComparer : IComparer<ListViewGroup>
    {
        public ListViewGroupComparer(SortOrder order)
        {
            this.sortOrder = order;
        }

        public int Compare(ListViewGroup x, ListViewGroup y)
        {
            // If we know how to compare the tags, do that.
            // Otherwise do a case insensitive compare on the group header.
            // We have explicitly catch the "almost-null" value of DBNull.Value,
            // since comparing to that value always produces a type exception.
            int result;
            IComparable comparable = x.Tag as IComparable;
            if (comparable != null && y.Tag != System.DBNull.Value)
                result = comparable.CompareTo(y.Tag);
            else
                result = String.Compare(x.Header, y.Header, true);

            if (this.sortOrder == SortOrder.Descending)
                result = 0 - result;

            return result;
        }

        private SortOrder sortOrder;
    }

    /// <summary>
    /// ColumnComparer is the workhorse for all comparison between two values of a particular column.
    /// If the column has a specific comparer, use that to compare the values. Otherwise, do
    /// a case insensitive string compare of the string representations of the values.
    /// </summary>
    /// <remarks><para>This class inherits from both IComparer and its generic counterpart
    /// so that it can be used on untyped and typed collections.</para></remarks>
    internal class ColumnComparer : IComparer, IComparer<OLVListItem>
    {
        public ColumnComparer(OLVColumn col, SortOrder order)
        {
            this.column = col;
            this.sortOrder = order;
            this.secondComparer = null;
        }

        public ColumnComparer(OLVColumn col, SortOrder order, OLVColumn col2, SortOrder order2)
            : this(col, order)
        {
            // There is no point in secondary sorting on the same column
            if (col != col2)
                this.secondComparer = new ColumnComparer(col2, order2);
        }

        public int Compare(object x, object y)
        {
            return this.Compare((OLVListItem)x, (OLVListItem)y);
        }

        public int Compare(OLVListItem x, OLVListItem y)
        {
            int result = 0;
            object x1 = this.column.GetValue(x.RowObject);
            object y1 = this.column.GetValue(y.RowObject);

            if (this.sortOrder == SortOrder.None)
                return 0;

            // Handle nulls. Null values come last
            bool xIsNull = (x1 == null || x1 == System.DBNull.Value);
            bool yIsNull = (y1 == null || y1 == System.DBNull.Value);
            if (xIsNull || yIsNull) {
                if (xIsNull && yIsNull)
                    result = 0;
                else
                    result = (xIsNull ? -1 : 1);
            } else {
                result = this.CompareValues(x1, y1);
            }

            if (this.sortOrder == SortOrder.Descending)
                result = 0 - result;

            // If the result was equality, use the secondary comparer to resolve it
            if (result == 0 && this.secondComparer != null)
                result = this.secondComparer.Compare(x, y);

            return result;
        }

        public int CompareValues(object x, object y)
        {
            // Force case insensitive compares on strings
            if (x is String)
                return String.Compare((String)x, (String)y, true);
            else {
                IComparable comparable = x as IComparable;
                if (comparable != null)
                    return comparable.CompareTo(y);
                else
                    return 0;
            }
        }

        private OLVColumn column;
        private SortOrder sortOrder;
        private ColumnComparer secondComparer;
    }

    #endregion

}

# ZippyClip
Clipboard History Viewer for Microsoft Windows.

## Usage
If the app is running, its window is usually hiden. Make it appear on screen at any time by pressing `Alt+Ctrl+V`.

    Alt+Ctr+V: Show ZippyClip window from anywhere in Windows
	
The window will present you with the list of the most recently copied items.
Navigate the list using the arrows keys or with the mouse.

	Numbers 1 through 9: Copy the n-th most recent item to the clipboard 
	                     (see index numbers to the left of the item)
	Strg+C:              Copy selected item to clipboard
	Enter:               Copy selected item to clipboard; 
	                     close ZippyClip window;
			     paste clipboard contents into foreground window
	Strg+Enter:          Perform alternative action if available
	Esc:                 Close ZippyClip window
	Ctrl+Q:              Quit ZippyClip
	Ctrl+,:				 Show settings
	
Performing any of the above actions will also close the ZippyClip window, except for accessing the settings.

ZippyClip comes with a try icon that will make the window appear when clicked. When right-clicked, it will 
display a context menu with commands that include pausing, settings.

## Alternative Actions
All items in the list can be copied to the clipboard. 

Additional actions can be performed on some kinds of items.
Read about how to perform those alternative actions in "Usage".

Hyperlinks: Anything classified as a hyperlink (you can recognize those items by their underlined text) 
            will open in the default web browser or Windows Explorer, depending on the type of the link.

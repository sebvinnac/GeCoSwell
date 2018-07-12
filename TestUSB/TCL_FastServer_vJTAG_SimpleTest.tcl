##############################################################################################
############################# Basic vJTAG Interface ##########################################
##############################################################################################
#http://idlelogiclabs.com/2012/04/15/talking-to-the-de0-nano-using-the-virtual-jtag-interface/
 
#This portion of the script is derived from some of the examples from Altera
 
global usbblaster_name
global test_device
# List all available programming hardwares, and select the USBBlaster.
# (Note: this example assumes only one USBBlaster connected.)
# Programming Hardwares:
foreach hardware_name [get_hardware_names] {
#   puts $hardware_name
    if { [string match "USB-Blaster*" $hardware_name] } {
        set usbblaster_name $hardware_name
    }
}
 
puts "\nSelect JTAG chain connected to $usbblaster_name.\n";
 
# List all devices on the chain, and select the first device on the chain.
#Devices on the JTAG chain:
 
foreach device_name [get_device_names -hardware_name $usbblaster_name] {
#   puts $device_name
    if { [string match "@1*" $device_name] } {
        set test_device $device_name
    }
}
puts "\nSelect device: $test_device.\n";
 
# Open device
proc openport {} {
    global usbblaster_name
        global test_device
    open_device -hardware_name $usbblaster_name -device_name $test_device
}
 
# Close device.  Just used if communication error occurs
proc closeport { } {
    catch {device_unlock}
    catch {close_device}
}
 
proc set_LEDs {send_data} {
#    openport
#    device_lock -timeout 10000
    # Shift through DR.  Note that -dr_value is unimportant since we're not actually capturing the value inside the part, just seeing what shifts out
    puts "Send - $send_data"
    device_virtual_ir_shift -instance_index 0 -ir_value 1 -no_captured_ir_value
    set tdi [device_virtual_dr_shift -dr_value $send_data -instance_index 0  -length 18]
	#Use this if you want to read back the tdi while you shift in the new value
	
    device_virtual_dr_shift -dr_value $send_data -instance_index 0  -length 18 -no_captured_dr_value
 
    # Set IR back to 0, which is bypass mode
    device_virtual_ir_shift -instance_index 0 -ir_value 0 -no_captured_ir_value
 
#    closeport
	puts "receive - $tdi"
    return $tdi
}
 
##############################################################################################
 
##############################################################################################
################################# TCP/IP Server ##############################################
##############################################################################################
 
#Code Dervied from Tcl Developer Exchange - http://www.tcl.tk/about/netserver.html
 
proc Start_Server {port} {
    set s [socket -server ConnAccept $port]
    puts "Started Socket Server on port - $port"
    vwait forever
}
 
proc ConnAccept {sock addr port} {
    global conn
 
    # Record the client's information
 
    puts "Accept $sock from $addr port $port"
    set conn(addr,$sock) [list $addr $port]
    # Ensure that each "puts" by the server
    # results in a network transmission
 
    fconfigure $sock -buffering line
	
	openport
	device_lock -timeout 10000
 
    # Set up a callback for when the client sends data
 
    fileevent $sock readable [list IncomingData $sock]
}
 
proc IncomingData {sock} {
    global conn
 
    # Check end of file or abnormal connection drop,
    # then write the data to the vJTAG
	
    if {[eof $sock] || [catch {gets $sock line}]} {
    close $sock
    puts "Close $conn(addr,$sock)"
    unset conn(addr,$sock)
	closeport
    } else {
    #Before the connection is closed we get an emtpy data transmission. Let's check for it and trap it
    set data_len [string length $line]
    if {$data_len != "0"} then {
        #Extract the First Bit
        set line [string range $line 0 17]
        #Send the vJTAG Commands to Update the LED
        puts $sock [set_LEDs $line]
#		flush $sock
		puts "fin"
    }
    }
}
 
#Start that Server at Port 2540
Start_Server 2540
 
##############################################################################################
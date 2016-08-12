    print("ESP8266 Server1")
--wifi.setmode(wifi.STATION) 
IPADR = "192.168.43.126" --Requested static IP address for the ESP
--IPROUTER = "192.168.0.9" --IP address for the Wifi router
wifi.sta.setip({ip=IPADR,netmask="255.255.255.0"})
wifi.sta.config("iLights","12345678") -- connecting to server
wifi.sta.connect() 
print("Server IP Address:",wifi.sta.getip())

flag = true
lastOn = "-1"
startGame = false
changesCounter = 0

server = net.createServer(net.TCP) 

print("listening...")

server:listen(80, function(conn)
    conn:on("receive", function(conn, receivedData)
    print ("Recived data -" .. receivedData .. "-")

    if (receivedData == "over") then
           startGame = false
           flag = true
           lastOn = -1
           conn:send("0" + changesCounter)
    end
    
    if (startGame) then
        if (flag and (receivedData ~= lastOn) and (receivedData ~= "start")) then
            print("Sending on ")
            flag = false
            conn:send("on")
            lastOn = receivedData
        elseif (receivedData == lastOn) then 
            print("changed Flag ")
            flag = true 
            changesCounter = changesCounter + 1
            lastOn = -1
            conn:close()
        else
           print("closing conn")
           conn:close()
        end
    else
        if (receivedData == "start") then
             changesCounter = 0
             startGame = true
        end
        
        print("closing conn")
        conn:close()
    end
    end)
    conn:on("sent", function(conn)
        collectgarbage() 
    end)
end) 
    

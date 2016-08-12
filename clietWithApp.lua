-- CODE FOR CLIENT ESP
print("ESP8266 Client")
wifi.setmode(wifi.STATION) 
IPADR = "192.168.43.151" --Requested static IP address for the ESP
IPROUTER = "192.168.43.1" --IP address for the Wifi router
wifi.sta.setip({ip=IPADR,netmask="255.255.255.0",gateway=IPROUTER})
wifi.sta.config("iLights","12345678") -- connecting to server



wifi.sta.connect() 
 lightOn=false
pin1=0
pin2=5
pin3=6
pin4=1
pin5=4
pin6=2


flag = true
--sets the pin to be for output
gpio.mode(pin1,gpio.OUTPUT)
gpio.write(pin1,gpio.LOW)

gpio.mode(pin2,gpio.OUTPUT)
gpio.write(pin2,gpio.LOW)

gpio.mode(pin3,gpio.OUTPUT)
gpio.write(pin3,gpio.LOW)

gpio.mode(pin4,gpio.OUTPUT)
gpio.write(pin4,gpio.LOW)

gpio.mode(pin5,gpio.OUTPUT)
gpio.write(pin5,gpio.LOW)

gpio.mode(pin5,gpio.OUTPUT)
gpio.write(pin5,gpio.LOW)

gpio.mode(pin6,gpio.OUTPUT)
gpio.write(pin6,gpio.LOW)

print("Looking for a connection")


tmr.alarm(2, 1000, tmr.ALARM_AUTO, function()
    if (wifi.sta.getip() ~= nil) then 
        if (flag) then 
            print("connected to wifi ")
            sk = net.createConnection(net.TCP, 0)
            sk:connect(80, "192.168.43.126") -- server ESP IP
            sk:on("connection", function(sk) 
                print("socket opened")
                sk:send("5") 
                flag = false
            end)
        end
        
      if (lightOn) then
          num = adc.readvdd33(0)
            print(num)
             while ( num > 3500 ) do
                print(num)
                tmr.delay(5000)
                num = adc.readvdd33(0) 
             end
          print("light Off" )
          gpio.write(pin1,gpio.LOW)
          gpio.write(pin2,gpio.LOW)
          gpio.write(pin3,gpio.LOW)
          gpio.write(pin4,gpio.LOW)
          gpio.write(pin5,gpio.LOW)
          gpio.write(pin6,gpio.LOW)
          --tmr.delay(500000)

          flag = true
          sk:close() 
          print("sent done" ) 
          lightOn = false  
          print("after" ) 
      end


        
        sk:on("receive", function(sk, receivedData)
            print("received: " .. receivedData) 
                print("light On" ) 
                lightOn = true
                gpio.write(pin1,gpio.HIGH)  
                gpio.write(pin2,gpio.HIGH)
                gpio.write(pin3,gpio.HIGH)
                gpio.write(pin4,gpio.HIGH)
                gpio.write(pin5,gpio.HIGH)
                gpio.write(pin6,gpio.HIGH)  
        end)
        
        sk:on("disconnection", function(sk, receivedData)
            print("disconnection")
            flag=true            
        end)
        sk:on("sent", function(sk, receivedData)
            print("sent")
            collectgarbage()            
        end)
        
    else
        print("no connection...")
    end
end)

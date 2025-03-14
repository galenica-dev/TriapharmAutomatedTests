To use the GzipMessageEncoder, use the examples below to build and modify your configuration file:

1) Register the extension by adding the following XML within the "configuration/system.serviceModel" tag:
    <extensions>
      <bindingExtensions>
        <add name="gzipNetTcpBinding" type="Triamun.ActivePos.WcfHelper.Compression.Configuration.GZipNetTcpBindingCollectionElement, WcfHelper"/>
      </bindingExtensions>
    </extensions>

2) Under the "configuration/system.serivceModel/binding" tag, define the gzipNetTcpBinding as you would do for the netTcpBinding.
   The inner "binding" element is the same than the one of the "netTcpBinding" element:
	<gzipNetTcpBinding>
		<binding name="GZipTcpBinding" openTimeout="00:00:02" closeTimeout="00:00:02" sendTimeout="00:00:30" receiveTimeout="02:00:00" maxBufferPoolSize="80000" maxBufferSize="104857600" maxReceivedMessageSize="104857600">
		  <readerQuotas maxArrayLength="104857600" maxBytesPerRead="104857600" maxStringContentLength="104857600"/>
		  <security mode="None"/>
		</binding>
	</gzipNetTcpBinding>

3) Configure the endpoints (client and server) by setting the binding to "gzipNetTcpBinding" and define the bindingConfiguration to the configuration defined at 2).
   The service url scheme must be "net.tcp".


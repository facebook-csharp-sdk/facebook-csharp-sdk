<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="Facebook.Cloud" generation="1" functional="0" release="0" Id="97302dac-23b2-45a2-8cd0-245361d9ab11" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="Facebook.CloudGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="CSMvc3FacebookApp.Web:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/Facebook.Cloud/Facebook.CloudGroup/LB:CSMvc3FacebookApp.Web:Endpoint1" />
          </inToChannel>
        </inPort>
        <inPort name="CSMvc3FacebookApp.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp">
          <inToChannel>
            <lBChannelMoniker name="/Facebook.Cloud/Facebook.CloudGroup/LB:CSMvc3FacebookApp.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="CSMvc3FacebookApp.WebInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/Facebook.Cloud/Facebook.CloudGroup/MapCSMvc3FacebookApp.WebInstances" />
          </maps>
        </aCS>
        <aCS name="Certificate|CSMvc3FacebookApp.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" defaultValue="">
          <maps>
            <mapMoniker name="/Facebook.Cloud/Facebook.CloudGroup/MapCertificate|CSMvc3FacebookApp.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </maps>
        </aCS>
        <aCS name="CSMvc3FacebookApp.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/Facebook.Cloud/Facebook.CloudGroup/MapCSMvc3FacebookApp.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </maps>
        </aCS>
        <aCS name="CSMvc3FacebookApp.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="">
          <maps>
            <mapMoniker name="/Facebook.Cloud/Facebook.CloudGroup/MapCSMvc3FacebookApp.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </maps>
        </aCS>
        <aCS name="CSMvc3FacebookApp.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="">
          <maps>
            <mapMoniker name="/Facebook.Cloud/Facebook.CloudGroup/MapCSMvc3FacebookApp.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </maps>
        </aCS>
        <aCS name="CSMvc3FacebookApp.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="">
          <maps>
            <mapMoniker name="/Facebook.Cloud/Facebook.CloudGroup/MapCSMvc3FacebookApp.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </maps>
        </aCS>
        <aCS name="CSMvc3FacebookApp.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/Facebook.Cloud/Facebook.CloudGroup/MapCSMvc3FacebookApp.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </maps>
        </aCS>
        <aCS name="CSMvc3FacebookApp.Web:?IsSimulationEnvironment?" defaultValue="">
          <maps>
            <mapMoniker name="/Facebook.Cloud/Facebook.CloudGroup/MapCSMvc3FacebookApp.Web:?IsSimulationEnvironment?" />
          </maps>
        </aCS>
        <aCS name="CSMvc3FacebookApp.Web:?RoleHostDebugger?" defaultValue="">
          <maps>
            <mapMoniker name="/Facebook.Cloud/Facebook.CloudGroup/MapCSMvc3FacebookApp.Web:?RoleHostDebugger?" />
          </maps>
        </aCS>
        <aCS name="CSMvc3FacebookApp.Web:?StartupTaskDebugger?" defaultValue="">
          <maps>
            <mapMoniker name="/Facebook.Cloud/Facebook.CloudGroup/MapCSMvc3FacebookApp.Web:?StartupTaskDebugger?" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:CSMvc3FacebookApp.Web:Endpoint1">
          <toPorts>
            <inPortMoniker name="/Facebook.Cloud/Facebook.CloudGroup/CSMvc3FacebookApp.Web/Endpoint1" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:CSMvc3FacebookApp.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput">
          <toPorts>
            <inPortMoniker name="/Facebook.Cloud/Facebook.CloudGroup/CSMvc3FacebookApp.Web/Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </toPorts>
        </lBChannel>
        <sFSwitchChannel name="SW:CSMvc3FacebookApp.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp">
          <toPorts>
            <inPortMoniker name="/Facebook.Cloud/Facebook.CloudGroup/CSMvc3FacebookApp.Web/Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
          </toPorts>
        </sFSwitchChannel>
      </channels>
      <maps>
        <map name="MapCSMvc3FacebookApp.WebInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/Facebook.Cloud/Facebook.CloudGroup/CSMvc3FacebookApp.WebInstances" />
          </setting>
        </map>
        <map name="MapCertificate|CSMvc3FacebookApp.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" kind="Identity">
          <certificate>
            <certificateMoniker name="/Facebook.Cloud/Facebook.CloudGroup/CSMvc3FacebookApp.Web/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </certificate>
        </map>
        <map name="MapCSMvc3FacebookApp.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/Facebook.Cloud/Facebook.CloudGroup/CSMvc3FacebookApp.Web/Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </setting>
        </map>
        <map name="MapCSMvc3FacebookApp.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" kind="Identity">
          <setting>
            <aCSMoniker name="/Facebook.Cloud/Facebook.CloudGroup/CSMvc3FacebookApp.Web/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </setting>
        </map>
        <map name="MapCSMvc3FacebookApp.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" kind="Identity">
          <setting>
            <aCSMoniker name="/Facebook.Cloud/Facebook.CloudGroup/CSMvc3FacebookApp.Web/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </setting>
        </map>
        <map name="MapCSMvc3FacebookApp.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" kind="Identity">
          <setting>
            <aCSMoniker name="/Facebook.Cloud/Facebook.CloudGroup/CSMvc3FacebookApp.Web/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </setting>
        </map>
        <map name="MapCSMvc3FacebookApp.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/Facebook.Cloud/Facebook.CloudGroup/CSMvc3FacebookApp.Web/Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </setting>
        </map>
        <map name="MapCSMvc3FacebookApp.Web:?IsSimulationEnvironment?" kind="Identity">
          <setting>
            <aCSMoniker name="/Facebook.Cloud/Facebook.CloudGroup/CSMvc3FacebookApp.Web/?IsSimulationEnvironment?" />
          </setting>
        </map>
        <map name="MapCSMvc3FacebookApp.Web:?RoleHostDebugger?" kind="Identity">
          <setting>
            <aCSMoniker name="/Facebook.Cloud/Facebook.CloudGroup/CSMvc3FacebookApp.Web/?RoleHostDebugger?" />
          </setting>
        </map>
        <map name="MapCSMvc3FacebookApp.Web:?StartupTaskDebugger?" kind="Identity">
          <setting>
            <aCSMoniker name="/Facebook.Cloud/Facebook.CloudGroup/CSMvc3FacebookApp.Web/?StartupTaskDebugger?" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="CSMvc3FacebookApp.Web" generation="1" functional="0" release="0" software="C:\Projects\facebooksdk\Samples\CSAzureMvc3FacebookApp\bin\Debug\Facebook.Cloud.csx\roles\CSMvc3FacebookApp.Web" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp" portRanges="3389" />
              <outPort name="CSMvc3FacebookApp.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/Facebook.Cloud/Facebook.CloudGroup/SW:CSMvc3FacebookApp.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
                </outToChannel>
              </outPort>
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="" />
              <aCS name="?IsSimulationEnvironment?" defaultValue="" />
              <aCS name="?RoleHostDebugger?" defaultValue="" />
              <aCS name="?StartupTaskDebugger?" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;CSMvc3FacebookApp.Web&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;CSMvc3FacebookApp.Web&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" certificateStore="My" certificateLocation="System">
                <certificate>
                  <certificateMoniker name="/Facebook.Cloud/Facebook.CloudGroup/CSMvc3FacebookApp.Web/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
            </certificates>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/Facebook.Cloud/Facebook.CloudGroup/CSMvc3FacebookApp.WebInstances" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyID name="CSMvc3FacebookApp.WebInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="91687716-41a3-429d-a168-550f86126481" ref="Microsoft.RedDog.Contract\ServiceContract\Facebook.CloudContract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="b57ffde9-cecc-4e5c-a871-8152852fb4b3" ref="Microsoft.RedDog.Contract\Interface\CSMvc3FacebookApp.Web:Endpoint1@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/Facebook.Cloud/Facebook.CloudGroup/CSMvc3FacebookApp.Web:Endpoint1" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="3683343e-dd36-4163-aa89-c5d8e975ba50" ref="Microsoft.RedDog.Contract\Interface\CSMvc3FacebookApp.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/Facebook.Cloud/Facebook.CloudGroup/CSMvc3FacebookApp.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>
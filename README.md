# ArchitecturalShowcase
<h2>Problem Statement Taken</h2>
<br/>
<h4>One simple grid in UI capable of CRUD such as Add,Edit,Delete,Update<h4>
<br/>
<h2>Summary</h2>
 <br/>
<h4>Very Simple example of WebApi implimenting Cloud Native Architecture having capability to handle 10000 concurrent user with degree of parallalism having capability of controlling Task and Thread.</h4>

<h2>Highlights</h2>
<br/>
<h4>
 <ul>
<li>With abstract Generics implimentation of BaseController we can have typed Factory e.g. ILogger</li>
<li>Can handle 10000 concurrent user</li>
<li>Ability to control over Task and Thread with cancellation token implimentation</li>
<li>Through nTier layer with ASP .Net Core dependency Injection implimenting multiple resolver</li>
<li>Cloud Native design with API plugability support</li>
<li>Used mini ORM like dapper</li>
<li>Used EntityFrameworkCore in asynchronus way with granular division of transaction [Challenge: DbContext doesn't support async and EF is not good for transaction specific]</li>
  </ul>
 </h4>
<br/>
<h2>Future Scope</h2>
<br/>
<h4>
  <ul>
<li>Impliment Distributed Saga to fit in big Microservices mesh</li>
<li>Devide repository to basic unit of work</li>
<li>Add Serilog implimentation in Sync</li>
<li>Token Authetication</li>
<li>SSO capability with Claims implimentation</li>
<li>MongoDB or any Document DB integration</li>
<li>Integrate Azure Cosmos</li>
  </ul>
 </h4>
<br/>
<h2>Design Pattern Used</h2>
<br/>
<h4>
 <ul>
 <li>Saga</li>
 <li>AggregatorRoot[AR]</li>
 <li>CQRS[Command Query Responsibility Segregation]</li>
 <li>Repository</li>
 <li>DI[Dependency Injection]</li>
  </ul>
 </h4>
<br/>
<h2>ORM tools</h2>
<br/>
<h4>
EntityFramework Core[Code first] and Dapper
 <h4>
<br/>
<h2>Other Tools</h2>
<br/>
<h4>
<ul>
<li>Swagger</li>
<li>Automapper</li>
</ul>
</h4>
<br/>

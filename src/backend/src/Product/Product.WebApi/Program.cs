using HotChocolate.AspNetCore;
using Nest;
using Product.Core.Appplication;
using Product.Core.Domain.Repository;
using Product.Infrastructure.Orm.Repository;
using Product.WebApi.GraphQL;
using Product.WebApi.GraphQL.ProductType;
using Shared.Infrasctructure.Orm.Service;
using Shared.Infrastructure.Orm;
using StackExchange.Redis;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IElasticClient>(sp =>
{
    var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
        .DefaultIndex("products");
    return new ElasticClient(settings);
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(
        //typeof(GetAllProductQueryHandler).Assembly, // <=== Adicione essa linha
        typeof(ApplicationLayer).Assembly,
        typeof(Program).Assembly
    );
});


builder.Services
    .AddGraphQLServer()
    .AddQueryType<ProductQuery>()
    .AddMutationType<ProductMutation>()
    .AddType<ProductType>()
    .AddFiltering()
    .AddSorting();

builder.Services.AddScoped<ProductQuery>();
builder.Services.AddScoped<ProductMutation>();

builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);
builder.Services.AddDbContext<DefaultDbContext>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();

#if DEBUG
var apiUrls = builder.Configuration.GetSection("ConnectionStrings").Get<Dictionary<string, string>>();
builder.Services.AddSingleton<IConnectionMultiplexer>(x =>
{
    return ConnectionMultiplexer.Connect(apiUrls["Redis"]);
    //return ConnectionMultiplexer.Connect("localhost: 6379");    
});
#else
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("redis:6379"));

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80); // Porta do container
});
#endif
builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(cors => cors
.AllowAnyMethod()
.AllowAnyHeader()
.AllowAnyOrigin()
);

app.UseCors((g) => g.AllowAnyOrigin());
app.UseCors((g) => g.AllowCredentials());

app.MapControllers();

app.MapGraphQL("/api/v1/graphql");

app.MapGraphQL().WithOptions(new GraphQLServerOptions()
{
    Tool = { Enable = false }
});

app.Run();

using MediatR;
using Parqueadero.Application.Parqueadero.Commands;
using Parqueadero.Application.Parqueadero.Querys;
using Parqueadero.Domain.Dtos;

namespace Parqueadero.Api.ApiHandlers
{
    public static class ParqueaderoApi
    {
        public static RouteGroupBuilder MapParqueadero(this IEndpointRouteBuilder routeHandler)
        {
            routeHandler.MapGet("/", async (IMediator mediator) =>
            {
                return Results.Ok(await mediator.Send(new ParqueaderoQuery()));
            }).Produces(StatusCodes.Status200OK, typeof(List<ParqueaderoDto>));

            routeHandler.MapGet("/{placa}", async (IMediator mediator, string placa) =>
            {
                return Results.Ok(await mediator.Send(new ParqueaderoQueryxPlaca(placa)));
            }).Produces(StatusCodes.Status200OK, typeof(ParqueaderoDto));

            routeHandler.MapPost("/", async (IMediator mediator, ParqueaderoCreateCommand parqueadero) =>
            {
                var Vehiculo = await mediator.Send(parqueadero);
                return Results.Ok(Vehiculo);
            }).Produces(statusCode: StatusCodes.Status201Created);


            routeHandler.MapPut("/{placa}", async (IMediator mediator, string placa) =>
            {
                var result = await mediator.Send(new ParqueaderoUpdateCommand(placa));
                return Results.Ok(result);
            }).Produces(statusCode: StatusCodes.Status200OK);

            return (RouteGroupBuilder)routeHandler;

        }

    }
}

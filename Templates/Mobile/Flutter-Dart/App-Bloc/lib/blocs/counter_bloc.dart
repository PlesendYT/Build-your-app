/// BLoC pattern implementation for a counter with increment and decrement events.
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:equatable/equatable.dart';

/// Base event class for counter-related events.
abstract class CounterEvent extends Equatable {
  const CounterEvent();
  @override
  List<Object> get props => [];
}

/// Event fired to increase the counter by 1.
class IncrementEvent extends CounterEvent {}
/// Event fired to decrease the counter by 1.
class DecrementEvent extends CounterEvent {}

/// The state held by the counter BLoC, containing the current count.
class CounterState extends Equatable {
  final int count;
  const CounterState(this.count);
  @override
  List<Object> get props => [count];
}

/// BLoC that processes [IncrementEvent] and [DecrementEvent] to update the counter state.
class CounterBloc extends Bloc<CounterEvent, CounterState> {
  CounterBloc() : super(const CounterState(0)) {
    on<IncrementEvent>((event, emit) => emit(CounterState(state.count + 1)));
    on<DecrementEvent>((event, emit) => emit(CounterState(state.count - 1)));
  }
}

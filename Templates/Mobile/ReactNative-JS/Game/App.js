import React, { useState } from 'react';
import { View, StyleSheet, Dimensions, PanResponder } from 'react-native';
import GameEngine from 'react-native-game-engine';

// Current screen dimensions used for initial entity placement.
const { width, height } = Dimensions.get('window');

/** Root game component with player movement and coin collection. */
export default function App() {
  // Game entities: player (draggable) and coin (collectible).
  const [entities, setEntities] = useState({
    player: {
      position: { x: width / 2, y: height - 80 },
      size: 30,
      renderer: () => null,
    },
    coin: {
      position: { x: width / 2, y: 50 },
      size: 20,
      collected: false,
      renderer: () => null,
    },
  });

  /** Pan responder to let the user drag the player entity. */
  const panResponder = PanResponder.create({
    onStartShouldSetPanResponder: () => true,
    // Update the player's position to follow the user's finger.
    onPanResponderMove: (_, g) => {
      setEntities(prev => ({
        ...prev,
        player: { ...prev.player, position: { x: g.moveX, y: g.moveY } },
      }));
    },
  });

  /** Game-system update: check collision between player and coin. */
  const updateSystem = (entities, { touches }) => {
    const player = entities.player;
    const coin = entities.coin;

    // Only check collision if the coin hasn't been collected yet.
    if (!coin.collected) {
      // Euclidean distance between player and coin centers.
      const dx = player.position.x - coin.position.x;
      const dy = player.position.y - coin.position.y;
      // If the circles overlap, mark the coin as collected.
      if (Math.sqrt(dx * dx + dy * dy) < player.size + coin.size) {
        return { ...entities, coin: { ...coin, collected: true } };
      }
    }
    return entities;
  };

  return (
    // Pan handlers on the root view capture touch events for drag input.
    <View style={styles.container} {...panResponder.panHandlers}>
      <GameEngine
        systems={[updateSystem]}
        entities={entities}
        // Custom renderer to position player and coin views.
        renderer={(entities) => (
          <>
            {/* Player circle rendered at the player entity position */}
            <View
              style={[styles.player, {
                left: entities.player.position.x - entities.player.size / 2,
                top: entities.player.position.y - entities.player.size / 2,
              }]}
            />
            {/* Coin circle, hidden once collected */}
            {!entities.coin.collected && (
              <View
                style={[styles.coin, {
                  left: entities.coin.position.x - entities.coin.size / 2,
                  top: entities.coin.position.y - entities.coin.size / 2,
                }]}
              />
            )}
          </>
        )}
      />
    </View>
  );
}

/** Stylesheet for game entities and layout. */
const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: '#fff' },
  player: { position: 'absolute', width: 30, height: 30, borderRadius: 15, backgroundColor: 'blue' },
  coin: { position: 'absolute', width: 20, height: 20, borderRadius: 10, backgroundColor: 'gold' },
});
